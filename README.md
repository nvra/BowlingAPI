# BowlingAPI

10 Pin Bowling Game

Description:
This game contains 10 frames. Each frame will have ten pins. And each frame has 2 chances to throw for knocking down the 10 pins.
If the player knocks down all the 10 pins in 1st throw in a frame, then it is Strike and there will not be 2nd throw for that frame.
If the player knocks down all the 10 pins in 2 throws in a frame, then it is a Spare.
And there could be a foul where the count is 0.

For Strike, the total for that frame will be the total of the frame and the count of the next 2 throws.
For Spare, the total for that frame will be the total of the frame and the count of the next 1 throw.

If the player knocks down all the 10 pins in the 10th(last) frame, then an extra throw(3) will be given. 
And this count will be added to the total of the 10th frame.

  F1	|	 F2		|	 F3		|	 F4		|	 F5		|	 F6		|	 F7		|	 F8		|	 F9		|	 F10		
T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2	|	T1 T2 T3

Postman collection : BowlingApi.postman_collection.json and DBScript.sql are attached with the solution.
First run the DBScript.sql to create the necessary tables and procs.

How to Run the API:
1) We need to insert a player. The request is POST
http://localhost:64454/api/player/{name} - HTTPOST

2) This inserts the player and the response includes the player id
{
    "id": 1, -- playerId
    "name": "player_name"
}

OR if the player already exists, we can get the id of the player from the list of the players 
http://localhost:64454/api/player - HTTPGET
this includes all the Players registered and we can get the id of a player who wants to play the game.
[
    {
        "id": 1,
        "name": "player1"
    },
    {
        "id": 2,
        "name": "player2"
    }
]
OR search by name using http://localhost:64454/api/player/{name} - HTTPGET
lists all the names matching the parameter {name}

3) using this player id, we will start a new game
http://localhost:64454/api/startgame/{playerId} - HTTPPOST

4) This POST request creates a new game and the response includes the game id
{
    "id": 2, -- gameId
    "playerId": 1,
    "player": {
        "id": 1,
        "name": "player_name"
    }
}

5) using this gameId we will start the new game.
6) the request is http://localhost:64454/api/FrameThrowScore HTTPPOST
this accepts the below sample request in body
{
    "GameId" : 1,
    "FrameNum" : 1 ,
    "ThrowNum" : 1,
    "Score" : 3,
    "Foul" : "n"
}
Validations and Assumptions:
    1) Game id should be existing one.
    2) Frame number should be between 1 and 10.
    3) Thrown number should be only 1 and 2 and could be 3 for Frame 10 if its a strike/spare.
    4) score should be in the range 0-10
    5) 'y' or 'f' indicates it is foul and 'n' not foul. this field is optional. 
    If not included in the request, it considers as not foul.
    6) gameid, framenum, thrownum, score are mandatory
    7) frame numbers should be played in order. It is restricted to play frame 3 before 1 and 2.
    8) thrownum should be played in order. It is restricted to play throw2 before 1.
    9) If throw1 is a strike, throw2 is not allowed.
    10) for frame 10, throw 3 is allowed only if the total of first 2 throws is 10. 
    If throw1 is strike, throw2 is not allowed. The player should play extra chance throw3.
    11) sum of 2 throws in each frame should be <= 10

7) this POST request inserts the score for each throw and 
also updates the total for that frame and previous frames if they are either a strike/spare.

8) the response shows the updated score till the played frames.

Sample Response
{
    "playerName": "player name",
    "gameId": 1, -- game id
    "frames": [
        {
            "frameNum": 1,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 2,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 20
        },
        {
            "frameNum": 3,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "5"
                },
                {
                    "throwNum": 2,
                    "score": "SPARE"
                }
            ],
            "totalScore": 10
        },
        {
            "frameNum": 4,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "FOUL"
                }
            ],
            "totalScore": 0
        }
    ],
    "totalScore": 60
}

9) If we want to see the score at any point or later, we can get it from 

http://localhost:64454/api/gameScores/{gameId} HTTPGET

sample response :
{
    "playerName": "vani",
    "gameId": 1,
    "frames": [
        {
            "frameNum": 1,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 2,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 3,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 4,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 5,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 6,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 7,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 8,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 9,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                }
            ],
            "totalScore": 30
        },
        {
            "frameNum": 10,
            "throws": [
                {
                    "throwNum": 1,
                    "score": "STRIKE"
                },
                {
                    "throwNum": 2,
                    "score": "10"
                }
            ],
            "totalScore": 30
        }
    ],
    "totalScore": 300
}

10) We can delete any game by the gameId. It deleted all the scores related to that game.

http://localhost:64454/api/deleteGameScores/{gameId} - HTTPDELETE
