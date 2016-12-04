using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compile
{
    class Grammer
    {

        void VnFunction()
        {
            if(curToken.codeType ==CodeType.dataType)
            {
                match(curToken.codeType == CodeType.dataType);
                match(curToken.codeType == CodeType.identy);
                match(curToken.literal == "(");
                VnXingCan();
                match(curToken.literal == ")");
                VnBlockCode();
            }
            else
            {
                error();
            }
        }
        void VnXingCan()
        {
            if(curToken.codeType==CodeType.dataType)
            {
                match(curToken.codeType == CodeType.dataType);
                match(curToken.codeType == CodeType.identy);
                if(curToken.literal==",")
                {
                    match(curToken.literal == ",");
                    VnXingCan();
                }
            }
        }
        void VnBlockCode()
        {
            if(curToken.literal=="{")
            {
                match(curToken.literal == "{");
                VnMultiSentence();
                match(curToken.literal == "}");
            }
        }
        void VnMultiSentence()
        {
            if(curToken.codeType==CodeType.constant || curToken.codeType == CodeType.dataType|| curToken.codeType == CodeType.identy)
            {
                VnSentence();
                VnMultiSentence();
            }

        }

        void VnSentence()
        {
            if(curToken.codeType==CodeType.dataType)
            {
                VnShengming();
                match(curToken.literal == ";");
            }else if(curToken.codeType==CodeType.identy)
            {
                VnInvokeOrAssignOrIdenty();
                match(curToken.literal == ";");
            }
            else if(curToken.literal=="if" || curToken.literal=="while")
            {

            }
            else
            {
                error();
            }
        }
        void VnShiCan()
        {
            if( curToken.literal!=")")
            {
                VnExpression();
                if (curToken.literal == ",")
                {
                    match(curToken.literal == ",");
                    VnShiCan();
                }

            }
        }

        void VnInvokeOrAssignOrIdenty()
        {
            match(curToken.codeType == CodeType.identy);
            if (curToken.literal == "(")
            {
                match(curToken.literal == "(");
                VnShiCan();
                match(curToken.literal == ")");
            }else if(curToken.literal=="=")
            {
                match(curToken.literal == "=");
                if (curToken.codeType == CodeType.identy || curToken.codeType == CodeType.constant || curToken.codeType == CodeType.zifu)
                    VnExpression();
                else
                    error();
            }

        }

        void VnInvokeOrIdenty()
        {
            match(curToken.codeType == CodeType.identy);
            if (curToken.literal == "(")
            {
                match(curToken.literal == "(");
                VnShiCan();
                match(curToken.literal == ")");
            }
        }



        void VnExpression()
        {
            if(curToken.codeType == CodeType.identy|| curToken.codeType == CodeType.constant)
            {
                if (curToken.codeType == CodeType.identy)
                {
                    VnInvokeOrIdenty();
                }
                else if (curToken.codeType == CodeType.constant)
                {
                    match(curToken.codeType == CodeType.constant);
                }
                if ("+-*/".IndexOf(curToken.literal) >= 0)
                {
                    match("+-*/".IndexOf(curToken.literal) >= 0);
                    VnExpression();
                }else if("><".IndexOf(curToken.literal) >= 0)
                {
                    match("><".IndexOf(curToken.literal) >= 0);
                    if (curToken.codeType == CodeType.constant)
                    {
                        match(curToken.codeType == CodeType.constant);
                    }else
                    {
                        VnInvokeOrIdenty();
                    }
                }
            }else if(curToken.codeType==CodeType.zifu)
            {
                match(curToken.codeType == CodeType.zifu);
            }
            else if(curToken.literal==";"||curToken.literal==")"||curToken.literal==",")
            {

            }else
            {
                error();
            }
        }
        void VnJudgeExpression()
        {
            if(curToken.literal=="if"||curToken.literal=="while")
            {
                match(curToken.literal == "if" || curToken.literal == "while");
                match(curToken.literal == "(");
                VnExpression();
                match(curToken.literal == ")");
                VnBlockCode();
            }
        }

        void VnShengming()
        {
            if (curToken.codeType == CodeType.dataType)
            {
                match(curToken.codeType == CodeType.dataType);
                match(curToken.codeType == CodeType.identy);
                if (curToken.literal == "=")
                {
                    if (curToken.codeType == CodeType.identy || curToken.codeType == CodeType.constant || curToken.codeType == CodeType.zifu)
                    {
                        VnExpression();
                    }
                    else
                        error();

                }

                while (curToken.literal == ",")
                {
                    match(curToken.literal == ",");
                    match(curToken.codeType == CodeType.identy);
                    if (curToken.literal == "=")
                    {
                        if (curToken.codeType == CodeType.identy || curToken.codeType == CodeType.constant || curToken.codeType == CodeType.zifu)
                        {
                            VnExpression();
                        }
                        else
                            error();
                    }

                }

            }
        }

        void VnAssign()
        {

        }
        token curToken;
        string returnMsg = "";
        LexicalAnalysis lex;
        int tokenindex = 0;
        bool getnexttoken()
        {
            if(tokenindex< lex.tokenList.Count)
            {
                curToken = lex.tokenList[tokenindex];
                return true;
            }
            return false;
        }
        void error()
        {

        }
        void match(bool ex)
        {
            if (ex)
                getnexttoken();
            else error();
        }

        public string run(LexicalAnalysis lexical )
        {

            return  "";
        }
    }
}
