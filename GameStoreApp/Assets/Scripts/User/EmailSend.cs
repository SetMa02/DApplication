using System;
using UnityEngine;
using  System.Net;
using  System.Net.Mail;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EmailSend : MonoBehaviour
    {
        [SerializeField] private FireBase _fireBase;
        [SerializeField] private ErrorMessage _errorMessage;
        [SerializeField] private CanvasGroup _mainWindow;
        private Button _button;
        private int summ = 0;
        private void Start()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(SendEmail);
        }
        private void SendEmail()
        {
            MailAddress fromAddress = new MailAddress("shop.games.ru19@outlook.com", "Shop");
            MailAddress destinationAddress = new MailAddress(_fireBase.User.Email, "User");
            MailMessage newMassage = new MailMessage(fromAddress, destinationAddress);

            newMassage.Subject ="Отмеченные игры";
            newMassage.Body = $"Здраствуйте, на вашем аккаунте есть {FavouriteGames.Games.Count} отмеченных игр, для " +
                              $"оформления доставки рекомендую связаться с нами по номеру телефона : +7987654321";
            foreach (var game in FavouriteGames.Games)
            {
                newMassage.Body += $"\n {game.Name}, цена {game.Privce};";
                summ += game.Privce;
            }

            newMassage.Body += $"\n Сумма избранных = {summ}";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromAddress.Address, "diplomShop19");
            
            smtpClient.Send(newMassage);
            
            _errorMessage.ShowMessage(_mainWindow, "Сообщение отправленно, проверте почту");

        }
    }
}