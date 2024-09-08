namespace Luciano.Serafim.Ebanx.Account.Api.Controllers
{
    /// <summary>
    /// balance data
    /// </summary>
    public class AccountBalanceDto
    {
        /// <summary>
        /// Account Id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Account Balance
        /// </summary>
        public double Balance { get; internal set; }
    }
}
