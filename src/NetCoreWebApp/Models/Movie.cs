using System.Diagnostics.CodeAnalysis;

namespace NetCoreWebApp.Models
{
    [SuppressMessage("Style", "IDE1006")]
    public class Movie
    {
        public string color { get; set; }
        public string director_name { get; set; }
        public int? num_critic_for_reviews { get; set; }
        public int? duration { get; set; }
        public int? director_facebook_likes { get; set; }
        public int? actor_3_facebook_likes { get; set; }
        public string actor_2_name { get; set; }
        public int? actor_1_facebook_likes { get; set; }
        public int? gross { get; set; }
        public string genres { get; set; }
        public string actor_1_name { get; set; }
        public string movie_title { get; set; }
        public int num_voted_users { get; set; }
        public int cast_total_facebook_likes { get; set; }
        public string actor_3_name { get; set; }
        public int? facenumber_in_poster { get; set; }
        public string plot_keywords { get; set; }
        public string movie_imdb_link { get; set; }
        public int? num_user_for_reviews { get; set; }
        public string language { get; set; }
        public string country { get; set; }
        public string content_rating { get; set; }
        public string budget { get; set; }
        public int? title_year { get; set; }
        public int? actor_2_facebook_likes { get; set; }
        public double imdb_score { get; set; }
        public string aspect_ratio { get; set; }
        public int movie_facebook_likes { get; set; }
    }
}