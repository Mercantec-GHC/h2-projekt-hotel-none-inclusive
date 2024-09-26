import { IoBedSharp, IoBookmarksSharp, IoTicketSharp } from "react-icons/io5";
import NavbarLink from "./Navbar Links/NavbarLink.jsx";
import './Navbar.css';
import { useNavigate } from "react-router-dom";
import { useAuth } from '../../context/AuthContext';

function Navbar() {
    const navigate = useNavigate();
    const { isAuthenticated, logout } = useAuth();

    const handleOnLogoClick = () => navigate('/');
    const handleOnLoginButtonClick = () => navigate('/login');
    const handleOnSignupButtonClick = () => navigate('/signup');
    const handleOnLogoutButtonClick = () => {
        logout();
        navigate('/');
    };

    return (
        <div className="navbar-container">
            <div className="navbar-logo-section">
                <img
                    src="src/assets/Logo.png"
                    alt="Logo"
                    className="navbar-logo"
                    draggable="false"
                    onClick={handleOnLogoClick}
                />
            </div>
            <div className="nav-links">
                <NavbarLink icon={<IoBedSharp />} text="Værelser" link="/rooms" />
                <NavbarLink icon={<IoBookmarksSharp />} text="Mine Bookings" link="/bookings" />
                <NavbarLink icon={<IoTicketSharp />} text="Kontakt" link="/contact" />
                {isAuthenticated ? (
                    <button className="navbar-logout-button" onClick={handleOnLogoutButtonClick}>Logout</button>
                ) : (
                    <>
                        <button className="navbar-login-button" onClick={handleOnLoginButtonClick}>Login</button>
                        <button className="navbar-signup-button" onClick={handleOnSignupButtonClick}>Sign up</button>
                    </>
                )}
            </div>
        </div>
    );
}

export default Navbar;
