using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text;

namespace Dialogue.Parser
{
	public class ParseError
	{
		public string Msg { get; private set; }
		public ParseError(string message)
		{
			Msg = message;
		}
	}

	public class Result<T>
	{
		public bool Success { get; private set; }
		public T Value { get; private set; }
		public ParseError Error { get; private set; }
		public int Consumed { get; private set; }

		public Result(ParseError error)
		{
			Success = false;
			Error = error;
			Consumed = 0;
		}

		public Result(T value, int consumed)
		{
			Success = true;
			Value = value;
			Consumed = consumed;
		}
	}

	abstract class Parser<T>
	{
		public abstract Result<T> Parse(string input, int index = 0);

		protected void CheckInputValid(string input, int index)
		{
			if(input == null)
			{
				throw new System.ArgumentNullException("input");
			}
			else if(index < 0 || index >= input.Length)
			{
				throw new System.ArgumentOutOfRangeException("index", "Substring index out of range");
			}
		}
	}

	class Or<T> : Parser<T>
	{
		Parser<T>[] options;

		public Or(params Parser<T>[] list)
		{
			if(list.Length < 2)
			{
				throw new System.ArgumentOutOfRangeException("list", "Or requires at least two parsers");
			}

			options = list;
		}

		public override Result<T> Parse(string input, int index = 0)
		{
			CheckInputValid(input, index);
			foreach(Parser<T> parser in options)
			{
				Result<T> result = parser.Parse(input, index);
				if (result.Success)
				{
					return result;
				}
			}
			return new Result<T>(new ParseError("All options failed"));
		}
	}

	class Many<T> : Parser<IEnumerable<T>>
	{
		bool _requireOnce;
		Parser<T> _parser;

		public Many(Parser<T> parser, bool requireOnce)
		{
			_requireOnce = requireOnce;
			_parser = parser;
		}
		public override Result<IEnumerable<T>> Parse(string input, int index = 0)
		{
			CheckInputValid(input, index);
			//TODO build up list of results
			throw new System.NotImplementedException();
		}
	}
}
