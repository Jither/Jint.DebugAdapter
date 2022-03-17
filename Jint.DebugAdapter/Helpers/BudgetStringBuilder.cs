namespace Jint.DebugAdapter.Helpers
{
    // Inspired by https://github.com/microsoft/vscode-js-debug/
    public class BudgetStringBuilder
    {
		private const string ellipsis = "…";
		private readonly List<string> tokens = new();
		private int _budget;
		private readonly string separator;

		public int Budget => _budget - (tokens.Count > 0 ? separator.Length : 0);
		public bool IsEmpty => tokens.Count == 0;

		public BudgetStringBuilder(int budget, string separator = "")
		{
			this.separator = separator;
			this._budget = budget - 1 - this.separator.Length;
		}

		public void Append(string text)
		{
			if (text.Length > Budget)
			{
				AppendEllipsis();
				return;
			}
			InternalAppend(text);
		}

		private void InternalAppend(string text)
		{
			if (tokens.Count > 0)
			{
				_budget -= separator.Length;
			}
			tokens.Add(text);
			_budget -= text.Length;
		}

		public void AppendEllipsis()
		{
			if (tokens[^1] != ellipsis)
			{
				tokens.Add(ellipsis);
			}
		}

		public bool CheckBudget()
		{
			if (_budget <= 0)
			{
				AppendEllipsis();
			}
			return _budget > 0;
		}

		public override string ToString()
		{
			return String.Join(separator, tokens);
		}
	}
}
