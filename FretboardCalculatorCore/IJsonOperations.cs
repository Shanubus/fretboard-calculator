namespace FretboardCalculatorCore
{
    public interface IJsonOperations
    {
        void LoadFromJsonString(string json);
        string ToJsonString();
    }
}