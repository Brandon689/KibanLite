using Moshi.MyAnimeList.Data;

AnimeDatabase db = new("Data Source=../../../../anime-2.db");
db.ImportFromJson("../../../../anime-offline-database.json");