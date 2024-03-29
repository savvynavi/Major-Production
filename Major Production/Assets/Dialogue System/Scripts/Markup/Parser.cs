using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Dialogue.Parser
{
	public class ParseError
	{
		// HACK might not need this
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

	public abstract class Parser<T>
	{
		public abstract Result<T> Parse(string input, int index = 0);

		protected void CheckInputValid(string input, int index)
		{
			if(input == null)
			{
				throw new System.ArgumentNullException("input");
			}
			else if(index < 0 || index > input.Length)
			{
				throw new System.ArgumentOutOfRangeException("index", "Substring index out of range");
			}
		}

		public Parser<IEnumerable<T>> many(bool requireOnce)
		{
			return new Many<T>(this, requireOnce);
		}

		public Parser<T> or(params Parser<T>[] list)
		{
			List<Parser<T>> totalList = new List<Parser<T>>();
			totalList.Add(this);
			totalList.AddRange(list);
			return new Or<T>(totalList.ToArray());
		}


	}

	public class Or<T> : Parser<T>
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

	public class Many<T> : Parser<IEnumerable<T>>
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
            int consumed = 0;
            List<T> results = new List<T>();
            // HACK figure out condition
            bool running = true;
            while (running)
            {
                Result<T> tryResult = _parser.Parse(input, index + consumed);
                if (tryResult.Success)
                {
                    results.Add(tryResult.Value);
                    consumed += tryResult.Consumed;
                    running = index + consumed < input.Length;
                }
                else
                {
                    running = false;
                }
            }
            if(results.Count == 0 && _requireOnce)
            {
                return new Result<IEnumerable<T>>(new ParseError("Many parser could not match any of input"));
            } else
            {
                return new Result<IEnumerable<T>>(results, consumed);
            }
		}
	}

	public class ManyDelimited<T> : Parser<IEnumerable<T>>
	{
		Parser<T> _parser;
		char _delimiter;
		public ManyDelimited(Parser<T> parser, char delimiter)
		{
			_parser = parser;
			_delimiter = delimiter;
		}

		public override Result<IEnumerable<T>> Parse(string input, int index = 0)
		{
			//TODO like Many, but require delimiter between
			CheckInputValid(input, index);
			// build up list of results
			int consumed = 0;
			List<T> results = new List<T>();
			// HACK figure out condition
			bool running = true;
			while (running)
			{
				Result<T> tryResult = _parser.Parse(input, index + consumed);
				if (tryResult.Success)
				{
					results.Add(tryResult.Value);
					consumed += tryResult.Consumed;
					//TODO if end reached, or no delimiter found, running = false;
					if(index + consumed < input.Length)
					{
						if(input[index + consumed] == _delimiter)
						{
							++consumed;
							running = index + consumed < input.Length;
						}
					} else
					{
						running = false;
					}
				}
				else
				{
					running = false;
				}
			}
			if (results.Count == 0 )
			{
				return new Result<IEnumerable<T>>(new ParseError("Many parser could not match any of input"));
			}
			else
			{
				return new Result<IEnumerable<T>>(results, consumed);
			}
		}
	}

	public class Between<T> : Parser<T>
	{
		char _before;
		char _after;
		Parser<T> _contents;

		public Between(char outside, Parser<T> contents)
		{
			_before = outside;
			_after = outside;
			_contents = contents;
		}

		public Between(char before, Parser<T> contents, char after)
		{
			_before = before;
			_after = after;
			_contents = contents;
		}

		public override Result<T> Parse(string input, int index = 0)
		{
			CheckInputValid(input, index);
			if (index == input.Length)
			{
				return new Result<T>(new ParseError("Unexpected end of input"));
			}
			int consumed = 0;
			if(input[index] != _before)
			{
				return new Result<T>(new ParseError("Starting quote did not match"));
			}
			consumed++;
			Result<T> contentsResult = _contents.Parse(input, index + consumed);
			if (!contentsResult.Success)
			{
				return contentsResult;
			}
			else
			{
				consumed += contentsResult.Consumed;
				if(index + consumed >= input.Length)
				{
					return new Result<T>(new ParseError("Unexpected end of input"));
				}
				else if (input[index + consumed] != _after)
				{
					return new Result<T>(new ParseError("Ending quote did not match"));
				}
				else
				{
					consumed++;
					return new Result<T>(contentsResult.Value, consumed);
				}

			}
		}
	}

	public class TrimWhitespace<T> : Parser<T>
	{
		Parser<T> _parser;
		public TrimWhitespace(Parser<T> parser)
		{
			_parser = parser;
		}

		public override Result<T> Parse(string input, int index = 0)
		{
			CheckInputValid(input, index);
			if (index == input.Length)
			{
				return new Result<T>(new ParseError("Unexpected end of input"));
			}
			int consumed = 0;
			// Trim initial whitespace
			while (index + consumed < input.Length && char.IsWhiteSpace(input[index + consumed]))
			{
				++consumed;
			}
			Result<T> testResult = _parser.Parse(input, index + consumed);
			if (testResult.Success)
			{
				consumed += testResult.Consumed;
				// Trim trailing whitespace
				while (index + consumed < input.Length && char.IsWhiteSpace(input[index + consumed]))
				{
					++consumed;
				}
				return new Result<T>(testResult.Value, consumed);
			} else
			{
				return testResult;
			}
		}
	}
}
