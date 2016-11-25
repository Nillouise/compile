﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace compile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        enum codeType
        {
            identy,
            opera,
            delimiter,
            constant
        }
        public class token
        {
            public string literal;
            int code;
            public token(int code,string literal)
            {
                this.code = code;
                this.literal = literal;
            }
        }



        class LexicalAnalysis
        {

            

            int sort()
            {
                char ch=' ';
                if(!get_nextchar(ref ch))
                {
                    string s="";
                    if (!get_line(ref s))
                        return 7;
                    else
                        return 0;
                }
                
                if (char.IsWhiteSpace(ch))
                    return 0;
                else if(char.IsLetter(ch))
                {
                    ungetchar();
                    return 3;
                }else if(char.IsDigit(ch))
                {
                    ungetchar();
                    return 1;
                }else if(ch=='/')
                {
                    char ch2=' ';
                    bool twoDelimer = get_nextchar(ref ch2);
                    ungetchar();
                    ungetchar();
                    if (get_nextchar(ref ch2) )
                    {
                        if(ch2=='/'||ch2=='*')
                        {
                            return 4;
                        }else
                        {
                            return 5;
                        }
                    }else
                    {
                        return 5;
                    }
                }else if(ch=='"'||ch=='\'' )
                {
                    ungetchar();
                    return 2;
                }else if(char.IsSymbol(ch))
                {
                    ungetchar();
                    return 2;
                }

                return 0;
            }
            int recog_dig()
            {
                int state = 0;
                string literal = "";
                while(state!=-1)
                {
                    char ch = ' ';
                    if (!get_nextchar(ref ch))
                        break;
                    switch (state)
                    {
                        case 0:
                            if (char.IsDigit(ch))
                                literal += ch;
                            else if (ch == '.')
                            {
                                literal += ch;
                                state = 1;
                            }
                            else if(char.IsLetter(ch))
                            {
                                state = 3;
                            }else
                            {
                                state = 2;
                            }
                            break;
                        case 1:
                            if (char.IsDigit(ch))
                                literal += ch;
                            else if (char.IsLetter(ch))
                                state = 3;
                            else
                                state = 2;
                            break;
                        case 2:
                            ungetchar();
                            tokenList.Add(new token(3, literal));
                            return 5;
                        case 3:
                            ungetchar();
                            tokenList.Add(new token(3, literal));
                            return 6;

                        default:
                            break;
                    }
                }

                return 0;
            }
            int recog_str()
            {
                int state = 0;
                string literal = "";
                while (state != -1)
                {
                    char ch = ' ';
                    if (!get_nextchar(ref ch))
                    {
                        return 6;
                    }
                    switch (state)
                    {
                        case 0:
                            if (ch == '"' )
                            { 
                                literal += ch;
                                state = 1;
                            }else if(ch=='\'')
                            {
                                literal += ch;
                                state = 3;
                            }
                            else
                            {
                                state = 101;
                            }

                            break;
                        case 1:
                            literal += ch;
                            if (ch!='\\')
                            {
                                if(ch=='"')
                                { 
                                    state = 100;
                                }
                            }else
                            {
                                state = 2;
                            }
                            break;
                        case 2:
                            literal += ch;
                            state = 1;
                            break;
                        case 3:
                            literal += ch;
                            if (ch == '\\')
                                state = 4;
                            else if (ch == '\'')
                            {
                                state = 100;
                            }
                            else
                                state = 6;
                            break;
                        case 4:
                            literal += ch;
                            state = 5;
                            break;
                        case 5:
                            if (ch != '\'')
                                state = 101;
                            else
                            {
                                literal += ch;
                                state = 100;
                            }
                            break;
                        case 6:
                            if (ch != '\'')
                                state = 101;
                            else
                            {
                                literal += ch;
                                state = 100;
                            }
                            break;
                            
                        case 100:
                            tokenList.Add(new token(4, literal));
                            return 0;
                        case 101:
                            tokenList.Add(new token(4, literal));
                            return 6;

                        default:
                            break;
                    }
                }

                return 0;
            }

            int recog_id()
            {
                int state = 0;
                string literal = "";
                while (state != -1)
                {
                    char ch = ' ';
                    if (!get_nextchar(ref ch))
                    {
                        tokenList.Add(new token(5, literal));
                        return 0;
                    }
                    switch (state)
                    {
                        case 0:
                            if (!char.IsLetter(ch))
                                return 6;
                            else
                            {
                                literal += ch;
                                state = 1;
                            }
                            break;
                        case 1:
                            if(char.IsLetterOrDigit(ch))
                            {
                                literal += ch;
                            }else
                            {
                                state = 100;
                            }
                            break;
                        case 100:
                            ungetchar();
                            tokenList.Add(new token(5, literal));
                            return 0;

                        default:
                            break;
                    }
                }

                return 0;
            }
            int hand_com()
            {
                char ch = ' ';
                if (!get_nextchar(ref ch))
                    return 6;
                if(ch!='\\')
                {
                    return 6;
                }
                char ch2 = ' ';
                if (!get_nextchar(ref ch2))
                    return 6;
                if (ch2 == '\\')
                {
                    while (get_nextchar(ref ch)) ;
                    
                    return 0;
                }else if(ch2=='*')
                {
                    string s = "";

                    do
                    {
                        int state = 0;
                        while (get_nextchar(ref ch))
                        {
                            switch (state)
                            {
                                case 0:
                                    if (ch == '*')
                                        state = 1;
                                    break;
                                case 1:
                                    if (ch == '\\')
                                        return 0;
                                    else
                                        state = 0;
                                    break;
                                default:
                                    break;
                            }
                        }
                    } while (get_line(ref s));
                    return 0;
                }
                else
                {
                    return 6;
                }
                
            }
            int recog_del()
            {
                char ch = ' ';
                if (!get_nextchar(ref ch))
                    return 6;
                if (!char.IsSymbol(ch)&&!char.IsPunctuation(ch))
                    return 6;
                if("<>&|^!%*/+-".IndexOf( ch)>=0)
                {
                    char ch2 = ' ';
                    get_nextchar(ref ch2);
                    if(ch2=='=')
                    {
                        tokenList.Add(new token(5, ch.ToString() + ch2));
                        return 0;
                    }else
                    {
                        if(ch==ch2 && "^!%*/".IndexOf(ch)<0)
                        {
                            tokenList.Add(new token(5, ch.ToString() + ch2));
                            return 0;
                        }
                        else
                        {
                            ungetchar();
                            tokenList.Add(new token(5, ch.ToString() ));
                            return 0;
                        }
                    }
                }else
                {
                    tokenList.Add(new token(5, ch.ToString()));
                    return 0;
                }
            }
            int error()
            {


                return 0;
            }


            string[] lines;
            int lineIndex;
            int charIndex;
            string errorMsg = "";
            List<token> tokenList;
            bool get_line(ref string receiveline)
            {
                if(lineIndex<lines.Length)
                {
                    receiveline = lines[lineIndex++];
                    charIndex = 0;
                    return true;
                }else
                {
                    return false;
                }
            }

            bool get_nextchar(ref char receiveChar)
            {
                if (lineIndex < lines.Length && charIndex< lines[lineIndex].Length)
                {
                    receiveChar = lines[lineIndex][charIndex];
                    charIndex++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            bool ungetchar()
            {
                charIndex--;
                if (charIndex < 0)
                {
                    charIndex = 0;
                    return false;
                }
                else
                {
                    return true;
                }
            }

            string lex(string source)
            {
                lines = source.Split('\n');

                int state = 0;

                switch (state)
                {
                    case 0:
                        state = sort();
                        break;
                    case 1:
                        state = recog_dig();
                        break;
                    case 2:
                        state = recog_str();
                        break;
                    case 3:
                        state = recog_id();
                        break;
                    case 4:
                        state = hand_com();
                        break;
                    case 5:
                        state = recog_del();
                        break;
                    case 6:
                        state = error();
                        break;

                    default:
                        break;
                }

                return "";
            }


        }





        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
