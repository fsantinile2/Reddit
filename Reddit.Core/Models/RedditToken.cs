﻿namespace Reddit.Core
{
    public class RedditToken
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string Expires_in { get; set; }
        public string Scope { get; set; }
    }
}
