using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using RPS;
using Xamarin.Forms;

namespace RockPaperScissorsMobile {
	public partial class MainPage : ContentPage {
		private RPSGame _game;
		private Dictionary<Pick, ImageSource> _pickImages;

		public MainPage() {
			GetPicksImages();
			InitializeComponent();
			SetDefaultPicksImages();
			SetAvatarsImages();
			SetButtonImages();
			_game = new RPSGame();
		}

		private void GetPicksImages() {
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
		}

		private void SetDefaultPicksImages() {
			PlayerPick.Source = QuestionMarkImage;
			ComputerPick.Source = QuestionMarkImage;
		}

		private static ImageSource QuestionMarkImage =>
			ImageSource.FromResource("RockPaperScissorsMobile.Images.question.png",
				Assembly.GetExecutingAssembly());

		private void SetAvatarsImages() {
			PlayerAvatar.Source = ImageSource.FromResource("RockPaperScissorsMobile.Images.user.png",
				Assembly.GetExecutingAssembly());
			ComputerAvatar.Source = ImageSource.FromResource("RockPaperScissorsMobile.Images.computer.png",
				Assembly.GetExecutingAssembly());
		}

		private void SetButtonImages() {
			RockButton.Source = _pickImages[Pick.Rock];
			ScissorsButton.Source = _pickImages[Pick.Scissor];
			PaperButton.Source = _pickImages[Pick.Paper];
		}

		private async void ScissorsButton_OnClicked(object sender, EventArgs e) {
			await PlayOneRound(Pick.Scissor);
		}

		private async void PaperButton_OnClicked(object sender, EventArgs e) {
			await PlayOneRound(Pick.Paper);
		}

		private async void RockButton_OnClicked(object sender, EventArgs e) {
			await PlayOneRound(Pick.Rock);
		}

		private async Task PlayOneRound(Pick playerPick) {
			var computerPick = _game.GetComputerPick(playerPick);
			GameStatus.IsVisible = true;
			UpdateGameStatus();

			UpdateScore();
			await UpdatePicksImages(playerPick, computerPick);
			if (_game.IsGameOver) {
				ChangePickButtonStatus(false);
			}
		}

		private void UpdateGameStatus() {
			if (_game.IsGameOver) {
				GameStatus.ImageSource = null;
				GameStatus.IsEnabled = true;
				GameStatus.Text = "Again?";
				GameStatus.Clicked += GameStatusOnClicked;
				return;
			}

			if (_game.CurrentRound.Winner == Player.Human) {
				GameStatus.Text = "You won";
				GameStatus.ImageSource = ImageSource.FromResource("RockPaperScissorsMobile.Images.award.png",
					Assembly.GetExecutingAssembly());
			}
			else if (_game.CurrentRound.Winner == Player.Computer) {
				GameStatus.Text = "You lost";
				GameStatus.ImageSource = ImageSource.FromResource("RockPaperScissorsMobile.Images.shocked.png",
					Assembly.GetExecutingAssembly());
			}
			else {
				GameStatus.Text = "It's a draw";
				GameStatus.ImageSource = ImageSource.FromResource("RockPaperScissorsMobile.Images.equal.png",
					Assembly.GetExecutingAssembly());
			}
		}

		private void GameStatusOnClicked(object sender, EventArgs e) {
			_game = new RPSGame();
			ChangePickButtonStatus(true);
			GameStatus.IsVisible = false;
			GameStatus.IsEnabled = false;
			SetDefaultPicksImages();
			UpdateScore();
		}

		private async Task UpdatePicksImages(Pick playerPick, Pick computerPick) {
			await Task.WhenAll(
				PlayerPick.FadeTo(0, 200),
				ComputerPick.FadeTo(0, 200)
				);
			PlayerPick.Source = _pickImages[playerPick];
			ComputerPick.Source = _pickImages[computerPick];
			await Task.WhenAll(
				PlayerPick.FadeTo(1, 200),
				ComputerPick.FadeTo(1, 200)
			);
		}

		private void ChangePickButtonStatus(bool status) {
			RockButton.IsEnabled = status;
			ScissorsButton.IsEnabled = status;
			PaperButton.IsEnabled = status;
		}

		private void UpdateScore() {
			PlayerScore.Text = _game.Score.HumanScore.ToString();
			ComputerScore.Text = _game.Score.ComputerScore.ToString();
		}

		private async void CreditsToolbarItem_OnClicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new CreditsPage());
		}
	}
}