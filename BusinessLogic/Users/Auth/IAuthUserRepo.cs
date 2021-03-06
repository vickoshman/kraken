﻿namespace BusinessLogic.Users.Auth
{
  public interface IAuthUserRepo : IRepo
  {
    int Save(AuthUser user);
    void Update(int id, AuthUser user);
    void Delete(AuthUser user);
    AuthUser ReadByEmail(string email);
    AuthUser ReadByUsermame(string username);
  }
}