﻿namespace TestConsole.CommandProcessing
{
  public class PostDislikeProcessor : CommandProcessor
  {
    public override string Process(string[] cmdParams)
    {
      var id = int.Parse(cmdParams[0]);
      var count = cmdParams.Length > 1 ? int.Parse(cmdParams[1]) : 1;

      for (int i = 0; i < count; i++)
        PostManager.Dislike(-1, id);

      return "Done!";
    }
  }
}