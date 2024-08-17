namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Creators";

            StringBuilder sb = new StringBuilder();

            ICollection<Creator> creatorToImport = new List<Creator>();

            ImportCreatorsXmlDtop[] deserializedCreators = xmlHelper.Deserialize<ImportCreatorsXmlDtop[]>(xmlString, xmlRoot);

            foreach (var creatorDto in deserializedCreators)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Boardgame> boardgames = new List<Boardgame>();

                foreach (ImportBoardgamesXmlDto boardgamesXmlDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgamesXmlDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame newBoardgame = new Boardgame()
                    {
                        Name = boardgamesXmlDto.Name,
                        Rating = boardgamesXmlDto.Rating,
                        YearPublished = boardgamesXmlDto.YearPublished,
                        CategoryType = (CategoryType)boardgamesXmlDto.CategoryType,
                        Mechanics = boardgamesXmlDto.Mechanics
                    };

                    boardgames.Add(newBoardgame);
                }

                Creator newCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                    Boardgames = boardgames
                };

                creatorToImport.Add(newCreator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, newCreator.FirstName, newCreator.LastName, newCreator.Boardgames.Count()));
            }
            context.Creators.AddRange(creatorToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportSellersJsonDto[] deserializedSellers = JsonConvert.DeserializeObject<ImportSellersJsonDto[]>(jsonString)!;

            ICollection<Seller> sellersToImport = new List<Seller>();

            var boardgameIds = context.Boardgames
                .Select(x=>x.Id)
                .ToArray();
             

            foreach (var sellersDto in deserializedSellers)
            {
                if (!IsValid(sellersDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellersDto.Name,
                    Address = sellersDto.Address,
                    Country = sellersDto.Country,
                    Website = sellersDto.Website
                };

                foreach (var boardgamesDto in sellersDto.BoardgamesId.Distinct())
                {
                    if (!boardgameIds.Contains(boardgamesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = boardgamesDto
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
           
                }
                sellersToImport.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }
            context.Sellers.AddRange(sellersToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
