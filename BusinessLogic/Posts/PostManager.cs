﻿using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Ratings;
using Common.Serialization;
using log4net;

namespace BusinessLogic.Posts
{
  public class PostManager
  {
    private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IPostRepo _postRepo;
    private readonly IRatingRepo _ratingRepo;

    private List<int> _trendingPostIds;

    public PostManager(IPostRepo postRepo, IRatingRepo ratingRepo)
    {
      Logger.Debug("Initializing...");

      _postRepo = postRepo;
      _ratingRepo = ratingRepo;
      _trendingPostIds = GenerateTrendingPosts();

      Logger.DebugFormat("Initialized with {0}, {1}", postRepo, ratingRepo);
    }

    private List<int> GenerateTrendingPosts()
    {
      return Enumerable.Range(0, 1000).ToList();
    }

    public Post Create(int userId, string text, string imageUrl)
    {
      var post = new Post(userId, text, imageUrl);

      Logger.DebugFormat("Creating post: {0}", post.ToJson());

      var id = _postRepo.Save(post);
      post.SetId(id);

      return post;
    }

    public void Update(Post post)
    {
      _postRepo.Update(post.Id, post);
    }

    #region Get

    public Post Get(int id)
    {
      return _postRepo.Read(id);
    }

    public List<Post> GetNextTrending(int idFrom, int count)
    {
      var ids = _trendingPostIds
        .SkipWhile(id => id != idFrom)
        .Take(count);

      return _postRepo.ReadAll(ids);
    }

    #endregion

    #region Ratings

    public Rating Like(int userId, int id)
    {
      var rating = new Rating(userId, RatingKindId.Like, RatingTargetKindId.Post, id);
      
      _ratingRepo.Save(rating);
      return rating;
    }

    public List<Rating> GetRatings(int postId)
    {
      return _ratingRepo.ReadByPostId(postId);
    }

    public Rating Dislike(int userId, int id)
    {
      var rating = new Rating(userId, RatingKindId.Dislike, RatingTargetKindId.Post, id);
      
      _ratingRepo.Save(rating);
      return rating;
    }

    public void RemoveRating(int userId, int id)
    {
      var rating = new Rating(userId, RatingTargetKindId.Post, id);
      _ratingRepo.Delete(rating);
    }

    #endregion

    public void Delete(int postId)
    {
      _postRepo.Delete(postId);
    }
  }
}