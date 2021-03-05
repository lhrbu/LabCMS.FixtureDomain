using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class RandomPasswordGenerator
    {
        public string Generate(int length=8)
        {
            Random random = new Random(DateTimeOffset.Now.Millisecond);
            Span<byte> buffer = stackalloc byte[length];
            for(int i=0;i<length;++i)
            {
                buffer[i]=Convert.ToByte(random.Next(97, 122));
            }
            return Encoding.ASCII.GetString(buffer);
        }
    }
}
