namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Creators";

            ExportCreatorsXmlDto[] creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorsXmlDto
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count(),
                    Boardgames = c.Boardgames
                    .Select(b => new ExportBoardgameXmlDto
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return xmlHelper.Serialize(creators, xmlRoot);
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            ExportSellersJsonDto[] exportSellerDto = context.Sellers
                .Where(x=>x.BoardgamesSellers.Any(bg=>bg.Boardgame.YearPublished>= year && bg.Boardgame.Rating<=rating))
                .Select(x=> new ExportSellersJsonDto
                {
                    Name = x.Name,
                    Website = x.Website,
                    Boardgames = x.BoardgamesSellers
                    .Where(bgs => bgs.Boardgame.YearPublished >= year && bgs.Boardgame.Rating <= rating)
                    .Select(bgs => new ExportBoardgameJsonDto 
                    {
                       Name =bgs.Boardgame.Name,
                       Rating =bgs.Boardgame.Rating,
                       Mechanics = bgs.Boardgame.Mechanics,
                       Category = bgs.Boardgame.CategoryType.ToString(),
                    })
                    .OrderByDescending(x=>x.Rating)
                    .ThenBy(x=>x.Name)
                    .ToArray()
                })
                .OrderByDescending(x=>x.Boardgames.Count())
                .ThenBy(x=>x.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(exportSellerDto, Formatting.Indented);

        }
    }
}