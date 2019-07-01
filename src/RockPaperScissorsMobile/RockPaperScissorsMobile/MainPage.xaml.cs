using System;
using System.Collections.Generic;
using System.Reflection;
using RPS;
using Xamarin.Forms;

namespace RockPaperScissorsMobile {
	public partial class MainPage : ContentPage {
		private readonly RPSGame _game;
		private readonly Dictionary<Pick, ImageSource> _pickImages;

		public MainPage() {
			_pickImages = new Dictionary<Pick, ImageSource>() {
				{
					Pick.Paper,
					ImageSource.FromResource("RockPaperScissorsMobile.Images.paper.png", Assembly.GetExecutingAssembly())
				}, {
					Pick.Rock,
					ImageSource.FromResource("RockPaperScissorsMobile.Images.rock.png", Assembly.GetExecutingAssembly())
				}, {
					Pick.Scissor,
					ImageSource.FromResource("RockPaperScissorsMobile.Images.scissors.png", Assembly.GetExecutingAssembly())
				}
			};
			InitializeComponent();
			PlayerAvatar.Source = ImageSource.FromResource("RockPaperScissorsMobile.Images.user.png",
				Assembly.GetExecutingAssembly());
			ComputerAvatar.Source = ImageSource.FromResource("RockPaperScissorsMobile.Images.computer.png",
				Assembly.GetExecutingAssembly());
			SetButtonImages();
			_game = new RPSGame();
		}

		private void SetButtonImages() {
			RockButton.ImageSource = _pickImages[Pick.Rock];
			ScissorsButton.ImageSource = _pickImages[Pick.Scissor];
			PaperButton.ImageSource = _pickImages[Pick.Paper];
		}

		private void ScissorsButton_OnClicked(object sender, EventArgs e) {
			PlayOneRound(Pick.Scissor);
		}

		private void PaperButton_OnClicked(object sender, EventArgs e) {
			PlayOneRound(Pick.Paper);
		}

		private void RockButton_OnClicked(object sender, EventArgs e) {
			PlayOneRound(Pick.Rock);
		}

		private void PlayOneRound(Pick playerPick) {
			var computerPick = _game.GetComputerPick(playerPick);
			UpdateScore();
			UpdatePicksImages(playerPick, computerPick);
			if (_game.IsGameOver) {
				DisableButton();
			}
		}

		private void UpdatePicksImages(Pick playerPick, Pick computerPick) {
			PlayerPick.Source = _pickImages[playerPick];
			ComputerPick.Source = _pickImages[computerPick];
		}

		private void DisableButton() {
			RockButton.IsEnabled = false;
			ScissorsButton.IsEnabled = false;
			PaperButton.IsEnabled = false;
		}

		private void UpdateScore() {
			PlayerScore.Text = _game.Score.HumanScore.ToString();
			ComputerScore.Text = _game.Score.ComputerScore.ToString();
		}
	}
}