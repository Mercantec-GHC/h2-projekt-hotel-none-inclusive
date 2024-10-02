import { IoBedSharp, IoBookmarksSharp, IoBusiness } from "react-icons/io5";
import { FaUser, FaPlusCircle } from "react-icons/fa";
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
                <NavbarLink icon={<FaPlusCircle />} text="Opret værelse" link="/create-room" />
                <NavbarLink icon={<IoBusiness />} text="Book værelse for kunde" link="/rooms" />
                <NavbarLink icon={<IoBedSharp />} text="Se alle værelser" link="/all-rooms" />
                <NavbarLink icon={<IoBookmarksSharp />} text="Bookings" link="/bookings" />
                <NavbarLink icon={<FaUser />} text="Brugere" link="/users" />
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
