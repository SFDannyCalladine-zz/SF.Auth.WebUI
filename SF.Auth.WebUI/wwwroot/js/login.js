/// <reference path="~/Scripts/jquery-1.8.2.js" />

var Login = new function ()
{
    var me = this;

    me.initialAction = null;
    me.processing = false;

    me.Initialise = function ()
    {
        me.AnimateChannelLogos();
    }

    me.AnimateChannelLogos = function ()
    {
        $('.channel-logos .logo-container').animate({ backgroundPosition: (parseInt($('.channel-logos .logo-container').css("backgroundPosition").replace('px', '')) - 1) + 'px' }, 50, me.AnimateChannelLogos);
    }
}

$(document).ready(Login.Initialise);
