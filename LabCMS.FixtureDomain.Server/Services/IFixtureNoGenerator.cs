using LabCMS.Seedwork.FixtureDomain;

namespace LabCMS.FixtureDomain.Server.Services
{
    public interface IFixtureNoGenerator
    {
        int Create(int index, Fixture fixture);
    }
}