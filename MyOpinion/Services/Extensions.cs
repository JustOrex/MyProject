
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyOpinion.Domain.Entities;
using System.Text.Json;

namespace MyOpinion.Service
{
   
    public static class Cutter
    {
        public static string CutController(this string str)
        {
            return str.Replace("Controller", "");
        }
    }

    public static class MySessionExtensions
    {

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }

        public static Dictionary<int, string> Errors = new()
        {
            [0] = "Что-то пошло не так",
            [1] = "Неверный пароль",
            [2] = "Пароли не совпадают",
            [3] = "Новый и старый пароль совпадают",
            [4] = "Такой логин уже существует",
            [5] = "Эта почта уже используется"
        };
    }

    public class BaseResponse
    {
        public bool State { get; set; } = false;

        public string Error { get; set; }

        
    }

    public class LiveSearcher
    {
        private IQueryable<Article> Articles;

        private List<string> ArticlesTitles = new();
        public LiveSearcher(IQueryable<Article> _Articles) 
        {
            Articles = _Articles;

            foreach (var Article in Articles)
            {
                ArticlesTitles.Add(Article.Title);
            }
        }



        public async Task<IQueryable<Article>> Search(string text)
        {
            var indexes = new List<int>();

            List<string> copyArticles = new();

            List<string> wordsInString = null;

            var finalArticlesTitles = new List<string>();

            foreach (var i in ArticlesTitles)
            {
                copyArticles.Add(i);
            }

            if (text.Contains(' '))
            {
                wordsInString = text.Split(' ').ToList();
            }

            for (int i = 0; i < copyArticles.Count; i++)
            {
                if (wordsInString == null)
                {
                    if (copyArticles[i].ToLower().Contains(text.ToLower().Trim()))
                    {
                        indexes.Add(i);
                        finalArticlesTitles.Add(copyArticles[i]);
                    }
                }
                else
                {
                    bool isWordSuit = true;

                    foreach (var word in wordsInString)
                    {
                        if (!copyArticles[i].ToLower().Contains(word.ToLower().Trim()))
                        {
                            isWordSuit = false;
                        }
                    }
                    if (isWordSuit)
                        indexes.Add(i);
                }
            }

            
            //foreach (var i in indexes) 
            //{
            //    finalArticlesTitles.Add(copyArticles[i]);
            //}

            List<Article> finalArticles = new();
            foreach (var articleTitle in finalArticlesTitles)
            {
                foreach(var article in Articles)
                {
                    if(article.Title == articleTitle)
                    {
                        finalArticles.Add(article);
                    }
                }
            }
            
            return finalArticles.AsQueryable();
        }
    }
        
    
}
