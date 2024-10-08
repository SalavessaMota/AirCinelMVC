using AirCinelMVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirCinelMVC.Helpers
{
    public class SeatHelper : ISeatHelper
    {
        public List<int> GetAvailableSeats(Flight flight)
        {
            var reservedSeats = flight.Tickets
                                    .Select(t => ConvertSeatStringToNumber(t.SeatNumber, flight.Airplane.Model))
                                    .ToList();

            var totalSeats = Enumerable.Range(1, flight.Airplane.Capacity).ToList();
            return totalSeats.Except(reservedSeats).ToList();
        }

        public string ConvertSeatNumber(int seatNumber, string airplaneModel)
        {
            int seatsPerRow = GetSeatsAndCorridorsPerRowByModel(airplaneModel);
            int[,] seatMap = GenerateSeatMap(airplaneModel, seatNumber);

            int seatCount = 0;
            for (int row = 0; row < seatMap.GetLength(0); row++)
            {
                int letterIndex = 0;
                for (int col = 0; col < seatMap.GetLength(1); col++)
                {
                    if (seatMap[row, col] == 1)
                    {
                        seatCount++;
                        char seatLetter = (char)('A' + letterIndex); 

                        if (seatCount == seatNumber)
                        {
                            return $"{row + 1}{seatLetter}";
                        }

                        letterIndex++;
                    }
                }
            }

            return string.Empty;
        }


        public int ConvertSeatStringToNumber(string seatString, string airplaneModel)
        {
            string rowPart = new string(seatString.TakeWhile(char.IsDigit).ToArray());
            char seatLetter = seatString.Last();

            int row = int.Parse(rowPart);
            int seatsPerRow = GetSeatsAndCorridorsPerRowByModel(airplaneModel);
            int seatPositionInRow = seatLetter - 'A';

            return (row - 1) * seatsPerRow + seatPositionInRow + 1;
        }

        public int GetSeatsAndCorridorsPerRowByModel(string model)
        {
            return model switch
            {
                "A319" => 7,  
                "A320" => 7,  
                "A330" => 10, 
                "A350" => 10, 
                "A380" => 12, 
                "737" => 7,   
                "747" => 12,  
                "757" => 7,   
                "767" => 9,   
                "777" => 10,  
                "E170" => 5,  
                "E175" => 5,  
                "E190" => 5,  
                "E195" => 5,  
                _ => 7        
            };
        }

        public int[,] GenerateSeatMap(string airplaneModel, int capacity)
        {
            int seatsPerRow = GetSeatsAndCorridorsPerRowByModel(airplaneModel);
            int totalRows = capacity / seatsPerRow;

            int[,] seatMap = new int[totalRows, seatsPerRow];

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < seatsPerRow; col++)
                {
                    seatMap[row, col] = 1;
                }
            }

            List<int> corridorPositions = GetCorridorPositions(seatsPerRow);
            foreach (int corridorPosition in corridorPositions)
            {
                for (int row = 0; row < totalRows; row++)
                {
                    seatMap[row, corridorPosition] = 0;
                }
            }

            return seatMap;
        }

        public List<int> GetCorridorPositions(int seatsPerRow)
        {
            List<int> corridorPositions = new List<int>();

            if (seatsPerRow < 8)
            {
                corridorPositions.Add(seatsPerRow / 2);
            }
            else if (seatsPerRow == 9)
            {
                corridorPositions.Add(2);
                corridorPositions.Add(6);
            }
            else if (seatsPerRow == 10)
            {
                corridorPositions.Add(2);
                corridorPositions.Add(7);
            }
            else if (seatsPerRow == 11)
            {
                corridorPositions.Add(3);
                corridorPositions.Add(7);
            }
            else if (seatsPerRow == 12)
            {
                corridorPositions.Add(3);
                corridorPositions.Add(8);
            }

            return corridorPositions;
        }
    }
}
