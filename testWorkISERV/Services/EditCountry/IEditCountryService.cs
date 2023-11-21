namespace testWorkISERV.Services.Country
{
    public interface IEditCountryService
    {
        public void AddCountry(string name);
        public void RemoveCountry(Guid id);
    }
}
