/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
* 
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to  permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/
//
// Samples.Search.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//
using System;
using System.Collections;
using Novell.Directory.Ldap.Utilclass;

namespace Novell.Directory.Ldap.Samples
{
    internal static class Search
    {
        public static void Run(string[] args)
        {
            if (args.Length != 6)
            {
                Usage();
                return;
            }

            string ldapHost = args[0];
            int ldapPort = Convert.ToInt32(args[1]);
            String loginDN = args[2];
            String password = args[3];
            String searchBase = args[4];
            String searchFilter = args[5];

            try
            {
                using (LdapConnection conn = new LdapConnection())
                {
                    Console.WriteLine("Connecting to:" + ldapHost);
                    conn.Connect(ldapHost, ldapPort);
                    conn.Bind(loginDN, password);
                    LdapSearchResults lsc = conn.Search(searchBase, LdapConnection.SCOPE_SUB, searchFilter, null, false);

                    while (lsc.hasMore())
                    {
                        LdapEntry nextEntry = null;

                        try
                        {
                            nextEntry = lsc.next();
                        }
                        catch (LdapException ex)
                        {
                            Printer.Error(ex.ToString());
                            // Exception is thrown, go for next entry
                            continue;
                        }

                        Console.WriteLine("--------------------------------------------------------------------------------");
                        Console.WriteLine(nextEntry.DN);
                        Console.WriteLine("--------------------------------------------------------------------------------");

                        LdapAttributeSet attributeSet = nextEntry.getAttributeSet();

                        IEnumerator ienum = attributeSet.GetEnumerator();

                        while (ienum.MoveNext())
                        {
                            LdapAttribute attribute = (LdapAttribute)ienum.Current;
                            string attributeName = attribute.Name;
                            string attributeVal = attribute.StringValue;

                            if (!Base64.isLDIFSafe(attributeVal))
                            {
                                byte[] tbyte = SupportClass.ToByteArray(attributeVal);
                                attributeVal = Base64.encode(SupportClass.ToSByteArray(tbyte));
                            }

                            Printer.PrintAttribute(attributeName, attributeVal);
                        }
                    }
                }
            }
            catch (LdapException ex)
            {
                Printer.Error(ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                Printer.Error(ex.ToString());
                return;
            }
        }

        public static void Usage()
        {
            Console.WriteLine("Usage:   mono Search <host name> <ldap port>  <login dn>" + " <password> <search base>" + " <search filter>");
            Console.WriteLine("Example: mono Search Acme.com 389" + " \"cn=admin,o=Acme\"" + " secret \"ou=sales,o=Acme\"" + "         \"(objectclass=*)\"");
        }
    }
}

