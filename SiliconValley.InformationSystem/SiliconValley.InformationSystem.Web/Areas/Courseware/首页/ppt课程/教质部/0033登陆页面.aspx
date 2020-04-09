<%@ Page Language="C#" AutoEventWireup="true" CodeFile="登陆页面.aspx.cs" Inherits="登陆页面" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .a{
            color:burlywood;
        }
        #cc{
     
            width:800px; 
            height:600px; 
            border:1px solid red;
            margin:0px auto;
      
            background-image:url(images/抽象手机模板-37133923.jpg);
        }
        .e{
            margin-right:10px;
            margin-left:48px;
        }
    </style>
  
    
</head>
<body>
    
	
    <form id="form1" runat="server">
        <div>
            <div style="border:1px solid red;">
                <img src="images/47n58PICbPw_1024.jpg"  style="width:100%; height:200px;"/>
            </div>
            <div style="margin:0px auto;  width:300px;">
               
          <h3>    <span>欢迎进入</span>
                <span style="color:red;">PanPan手机商城</span></h3>  
            </div>
            
                      <div>
                 

            <div id="cc" >
                
                    <table style="margin-top:200px; margin-left:200px;">
                      <tr>
                          <td colspan="2" >
                              
        <img src="images/22.png" style=" border-radius:50%;width: 60px;height:60px;margin-left:80px; border:1px solid red;"  alt=""/>
                            
                              
                          </td>
                      </tr>
                        <tr>
                            <td class="a">
                                <asp:Label ID="Label1" runat="server" Text="账号"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                           <tr>
                            <td class="a">
                                <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><asp:LinkButton ID="LinkButton1" runat="server" BorderColor="Red" OnClick="LinkButton1_Click">忘记密码？</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="Button1" runat="server" Text="登陆" CssClass="e"  OnClick="Button1_Click"/>
                                <asp:Button ID="Button2" runat="server" Text="注册" OnClick="Button2_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
