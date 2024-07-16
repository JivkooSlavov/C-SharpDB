namespace MusicHub
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {


            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .OrderByDescending(a=>a.Price)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                    .Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("f2"),
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.Writer)
                    .ToArray(),
                    AlbumPrice = a.Price.ToString("f2")
                }).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var a in albums)
            {
                sb.AppendLine($"-AlbumName: {a.Name}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");
                sb.AppendLine($"-Songs:");

                int numberOfSongs = 1;

                foreach (var item in a.Songs)
                {
                    sb.AppendLine($"---#{numberOfSongs}");
                    sb.AppendLine($"---SongName: {item.SongName}");
                    sb.AppendLine($"---Price: {item.Price}");
                    sb.AppendLine($"---Writer: {item.Writer}");
                    numberOfSongs++;
                }
                sb.AppendLine($"-AlbumPrice: {a.AlbumPrice}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToArray()
                .Where(s=>s.Duration.TotalSeconds > duration)
                .Select(s=> new 
                {
                   s.Name,
                    Performers = s.SongPerformers
                    .Select(sp=>$"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .OrderBy(p=>p)
                    .ToArray(),
                    WriterName = s.Writer.Name,
                   AlbumProducer = s.Album.Producer.Name,
                  Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToArray();
                
            StringBuilder sb = new StringBuilder();
            int numberOfSongs = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{numberOfSongs}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                foreach (var item in song.Performers)
                {
                    sb.AppendLine($"---Performer: {item}");
                    
                }
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");
                numberOfSongs++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
