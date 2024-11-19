using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRMAgreement.Data_Base;
using SRMAgreement.Class;
using Microsoft.EntityFrameworkCore;

namespace TestsCRM
{
    public class SearchTest
    {
        [Test]
        public void SearchInDb()
        {
            DataBaseContext context = new();

            var input = "";
            var expected = new List<_4D> { new _4D { Id = 1, NameGroup = "" } };

            // Act
            var result = Search.SearchInNameGroup<_4D>(context, d => d.NameGroup == input);

            // Assert
           
        }
    }
}
