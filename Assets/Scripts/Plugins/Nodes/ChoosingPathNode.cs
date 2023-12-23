using System;
using System.Linq.Expressions;
using UnityEngine;
using YG;

public class ChoosingPathNode : BaseNode, INodeRealizer
{
    [Input] public int input;
    [Output] public int output;
    [SerializeField] private string _condition;
    /// <summary>
    /// Parser for condition field
    /// Expression in condition should be like this
    /// !Andrey! > 1
    /// !Andrey! < 2 or !Slave! < 1
    /// !Andrey! = 0 and !Slave! = 0
    /// </summary>
    /// <exception cref="Exception"></exception>
    private BinaryExpression ParseCondition(string condition)
    {
        int left = condition.IndexOf("!");
        int right = condition.IndexOf("!", left+1);
        string tempCharacter = condition.Substring(left + 1, right - left - 1);
        if (!Enum.TryParse(tempCharacter, true, out Character result))
        {
            throw new Exception($"Character {tempCharacter} does not exist!");
        }
        int value = GetCharacterValue(result);
        string sign = condition.Substring(right + 1, 1);
        BinaryExpression body = null;
        if (int.TryParse(condition.Substring(right + 2), out right))
        {
            body = GetExpressionBySign(sign, value, right);
        }
        else
        {
            int or = condition.IndexOf("|");
            int and = condition.IndexOf("&");
            if (or != -1) 
            {
                return Expression.Or(
                    ParseCondition(condition.Substring(0, or)),
                    ParseCondition(condition.Substring(or)));
            }
            if (and != -1)
            {
                return Expression.And(
                    ParseCondition(condition.Substring(0, and)),
                    ParseCondition(condition.Substring(and)));
            }
        }
        return body;
    }
    public void StartParseCondition()
    {
        _condition = _condition.Replace("and", "&");
        _condition = _condition.Replace("or", "|").Replace(" ", "");
        var result = Expression.Lambda<Func<int, bool>>(
            ParseCondition(_condition), 
            Expression.Parameter(typeof(int)));
        _bus.Invoke(new ChoosingPathParsedSignal(result.Compile()(0)));
    }
    private int GetCharacterValue(Character ch)
    {
        switch (ch)
        {
            case Character.Andrey:
                return YandexGame.savesData.andreyCount;
            case Character.Slave:
                return YandexGame.savesData.slaveCount;
            case Character.Teacher:
                return YandexGame.savesData.teacherCount;
        }
        Debug.LogWarning($"Character {ch} does not take part in choosing the path.");
        return -1;
    }
    private BinaryExpression GetExpressionBySign(string sign, int left, int right)
    {
        switch (sign)
        {
            case "<":
                return Expression.LessThan(
                    Expression.Constant(left),
                    Expression.Constant(right));
            case ">":
                return Expression.GreaterThan(
                    Expression.Constant(left),
                    Expression.Constant(right));
            case "=":
                return Expression.Equal(
                    Expression.Constant(left),
                    Expression.Constant(right));
        }
        Debug.LogWarning($"The {sign} sign is not provided for use in the condition.");
        return null;
    }
    public void Implement()
    {

    }
}
