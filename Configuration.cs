﻿namespace ANPCentral
{
    public static class Configuration
    {
        public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU=";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "anp_central_IlTevUM/z0ey3NwCV/unWg==";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
