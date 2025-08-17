# EasyValidate Website Setup Script (PowerShell)
# This script clones and sets up the external EasyValidate documentation website repository

$WEBSITE_REPO_URL = "https://github.com/mu-dawood/EasyValidate-Website.git"  # Update this URL
$WEBSITE_DIR = "EasyValidate-Website"

Write-Host "🚀 Setting up EasyValidate Documentation Website..." -ForegroundColor Green

# Check if the website directory already exists
if (Test-Path $WEBSITE_DIR) {
    Write-Host "📁 Website directory already exists. Pulling latest changes..." -ForegroundColor Yellow
    Set-Location $WEBSITE_DIR
    git pull origin main
    Set-Location ..
} else {
    Write-Host "📥 Cloning EasyValidate Website repository..." -ForegroundColor Blue
    git clone $WEBSITE_REPO_URL $WEBSITE_DIR
}

# Check if clone/pull was successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Website repository setup complete!" -ForegroundColor Green
    Write-Host "📂 Website is available in: ./$WEBSITE_DIR" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "To start development:" -ForegroundColor White
    Write-Host "  cd $WEBSITE_DIR" -ForegroundColor Gray
    Write-Host "  npm install" -ForegroundColor Gray
    Write-Host "  npm run dev" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To build for production:" -ForegroundColor White
    Write-Host "  cd $WEBSITE_DIR" -ForegroundColor Gray
    Write-Host "  npm run build" -ForegroundColor Gray
} else {
    Write-Host "❌ Failed to setup website repository" -ForegroundColor Red
    Write-Host "Please check the repository URL and your internet connection" -ForegroundColor Red
    exit 1
}
