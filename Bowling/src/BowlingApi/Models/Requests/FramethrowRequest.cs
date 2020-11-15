using BowlingApi.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BowlingApi.Models.Requests
{
    public class FramethrowRequest : IValidatableObject
    {
        [Required]
        [GameIdCheck]
        public int GameId { get; set; }

        [Required]
        [Range(1, 10)]
        public int FrameNum { get; set; }

        [Required]
        [Range(1, 3)]
        public int ThrowNum { get; set; }

        [Required]
        [Range(0, 10)]
        public int Score { get; set; }

        [FoulCheck(ErrorMessage = "Foul accepts 'y' and 'f' for foul, 'n' for not foul")]
        public char Foul { get; set; }

        public bool IsFoul
        {
            get
            {
                if(char.ToUpper(Foul) == 'Y' || char.ToUpper(Foul) == 'F')
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsStrike
        {
            get
            {
                if (ThrowNum == 1 && Score == 10 && !IsFoul)
                    return true;

                return false;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (IBowlingService)validationContext.GetService(typeof(IBowlingService));
            if (FrameNum == 1 || (FrameNum != 1 && service.IsFrameNumValid(GameId, FrameNum)))
            {
                if (ThrowNum == 2)
                {
                    if (!service.IsThrowNumValid(GameId, FrameNum))
                    {
                        yield return new ValidationResult("Throw Num 1 doesn't exist.");
                    }
                    else if (service.IsThrowStrike(GameId, FrameNum))
                    {
                        if (FrameNum != 10)
                        {
                            yield return new ValidationResult("Throw Num 1 is a strike. Play next frame.");
                        }
                    }
                    else
                    {
                        var score1 = service.IsScoreValid(GameId, FrameNum);
                        if (score1 + Score > 10)
                        {
                            yield return new ValidationResult($"Throw1 score is {score1}. Invalid Throw2 score : {Score}. Total score should be 10.");
                        }
                    }
                }
                else if (ThrowNum == 3)
                {
                    if (FrameNum != 10)
                    {
                        yield return new ValidationResult($"Invalid ThrowNum for Frame {FrameNum}. ThrowNum can only be 1 and 2 for Frames 1 to 9.");
                    }
                    else if (!service.IsSpareForFrame10(GameId))
                    {
                        yield return new ValidationResult($"Invalid ThrowNum for Frame {FrameNum}. ThrowNum 3 is valid only if Frame 10 is a Spare.");
                    }
                }
            }
            else
            {
                yield return new ValidationResult($"FrameNum {FrameNum - 1} doesn't exist. Please play {FrameNum - 1} first.");
            }
        }
    }
}
