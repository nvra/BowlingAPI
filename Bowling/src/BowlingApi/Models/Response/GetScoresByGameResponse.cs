namespace BowlingApi.Models.Response
{
    public class GetScoresByGameResponse
    {
        public string PlayerName { get; set; }

        public int GameId { get; set; }

        public int FrameNum { get; set; }

        public int TotalScore { get; set; }

        public int ThrowNum { get; set; }

        public string Score { get; set; }
    }
}
