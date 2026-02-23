using Konyvtar_katalogus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konyvtar_katalogus.Service
{
    public class ScoreCalculated
    {
        public ScoreCalculated() { }

        public int scoreCalculateScore(Book book, string? title, string? author, string? isbn)
        {
            int score = 0;
            /*cím szerinti pontozás*/
            if (!!string.IsNullOrEmpty(title))
            {
                if (book.title == title) { score += 5; }
                else if (book.title.Contains(title)) { score += 2; }
            }

            /*szerző szerinti pontozás*/
            if (!string.IsNullOrEmpty(author)) { 
                if (book.author == author) { score += 3; }
                else if (book.author.Contains(author))
                {
                    score++;
                } }

            /*isbn szerinti pontozás*/
            if (!string.IsNullOrEmpty(isbn))
            {
                if (book.isbn == isbn) { score += 12; }
            }

            return score;
        }

    }
}
