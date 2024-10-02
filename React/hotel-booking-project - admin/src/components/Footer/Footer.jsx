import './Footer.css'

function Footer() {
    return (
        <footer className="footer-container">
            <div className="footer-logo-section">
                <img src="src/assets/Logo.png" alt="Logo" className="footer-logo" draggable="false" />
            </div>
            <div className="footer-text">
                <p>© 2024 None Inclusive. Alle rettigheder forbeholdes.</p>
                <p>Kontakt HR: <span>HR@HotelNoneInclusive.dk</span> | <span>+45 12 34 56 78</span></p>
            </div>
        </footer>
    )
}

export default Footer
