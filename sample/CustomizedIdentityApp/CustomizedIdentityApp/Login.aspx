<%@ Page Title="ログイン" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CustomizedIdentityApp.Login" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="body" runat="server">
  <div style="width:300px;">
    <div>
      <label for="userName">ユーザー名</label>
      <asp:TextBox ID="userName" runat="server" ClientIDMode="Static" />
      <asp:RequiredFieldValidator ID="userNameRequiredFieldValidator" runat="server"
        ControlToValidate="userName" ErrorMessage="ユーザー名を入力してください。"
        Display="None" />
    </div>
    <div>
      <label for="password">パスワード</label>
      <asp:TextBox ID="password" runat="server" TextMode="Password" ClientIDMode="Static" />
      <asp:RequiredFieldValidator ID="passwordRequiredFieldValidator" runat="server"
        ControlToValidate="password" ErrorMessage="パスワードを入力してください。"
        Display="None" />
    </div>
    <div style="padding-top:10px; text-align:center;">
      <asp:Button ID="login" runat="server" Text="ログイン" OnClick="login_Click" />
    </div>
  </div>
</asp:Content>
