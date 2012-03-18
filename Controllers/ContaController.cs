using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Twitterizer;

namespace QueroSaberParlamentar.Controllers
{
    public class ContaController : Controller
    {
        //
        // GET: /Conta/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string oauth_token, string oauth_verifier, string ReturnUrl)
        {
            if (string.IsNullOrEmpty(oauth_token) || string.IsNullOrEmpty(oauth_verifier))
            {
                UriBuilder builder = new UriBuilder(this.Request.Url);
                builder.Query = string.Concat(
                    builder.Query,
                    string.IsNullOrEmpty(ReturnUrl) ? string.Empty : "&",
                    "ReturnUrl=",
                    ReturnUrl);
                
                string token = OAuthUtility.GetRequestToken(
                    ConfigurationManager.AppSettings["TwitterConsumerKey"],
                    ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                    builder.ToString()).Token;

                return Redirect(OAuthUtility.BuildAuthorizationUri(token, true).ToString());
            }

            var tokens = OAuthUtility.GetAccessToken(
                ConfigurationManager.AppSettings["TwitterConsumerKey"],
                ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                oauth_token,
                oauth_verifier);

            //using (TwitterizerDbContext db = new TwitterizerDbContext())
            //{
            //    var user = db.Users.Find(tokens.UserId);
            //    if (user == null)
            //    {
            //        user = new User()
            //        {
            //            TwitterUserId = tokens.UserId,
            //            ScreenName = tokens.ScreenName,
            //            TwitterAccessKey = tokens.Token,
            //            TwitterAccessSecret = tokens.TokenSecret
            //        };

            //        db.Users.Add(user);

            //        db.SaveChanges();
            //    }

            //    FormsAuthentication.SetAuthCookie(user.ScreenName, false);
            //}

            FormsAuthentication.SetAuthCookie(tokens.ScreenName, false);

            if (string.IsNullOrEmpty(ReturnUrl))
                return Redirect("/");
            else
                return Redirect(ReturnUrl);

        }
    }
}
