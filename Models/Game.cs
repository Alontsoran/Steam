namespace Steam.Models
{
    public class Game
    {
        // שמות השדות הפרטיים
        private int appId;
        private string name;
        private DateTime releaseDate;
        private decimal price;
        private string description;
        private string headerImage;
        private string website;
        private bool windows;
        private bool mac;
        private bool linux;
        private int scoreRank;
        private string recommendations;
        private string publisher;

        static public List<Game> Gamelist = new List<Game>();

        // קונסטרקטור - שימוש באותם שמות כמו בפרופרטיז
        public Game(int appId1, string name1, DateTime releaseDate1, decimal price1,
            string description, string headerImage1, string website1,
            bool windows1, bool mac1, bool linux1,
            int scoreRank1, string recommendations, string publisher1)
        {
            appId = appId1;
            name = name1;
            releaseDate = releaseDate1;
            price = price1;
            this.description = description;
            headerImage = headerImage1;
            website = website1;
            windows = windows1;
            mac = mac1;
            linux = linux1;
            scoreRank = scoreRank1;
            this.recommendations = recommendations;
            publisher = publisher1;
        }

        // Properties - השארתי את אותם שמות כפי שהיו
        public int AppId1 { get => appId; set => appId = value; }
        public string Name1 { get => name; set => name = value; }
        public DateTime Releasedate1 { get => releaseDate; set => releaseDate = value; }
        public decimal Price1 { get => price; set => price = value; }
        public string Description { get => description; set => description = value; }
        public string HeaderImage1 { get => headerImage; set => headerImage = value; }
        public string Website1 { get => website; set => website = value; }
        public bool Windows1 { get => windows; set => windows = value; }
        public bool Mac1 { get => mac; set => mac = value; }
        public bool Linux1 { get => linux; set => linux = value; }
        public int ScoreRank1 { get => scoreRank; set => scoreRank = value; }
        public string Recommendations { get => recommendations; set => recommendations = value; }
        public string Publisher1 { get => publisher; set => publisher = value; }

    }
}