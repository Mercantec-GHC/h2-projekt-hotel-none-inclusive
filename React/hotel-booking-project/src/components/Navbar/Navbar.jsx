import { IoBedSharp } from "react-icons/io5";
import { IoBookmarksSharp } from "react-icons/io5";
import { IoTicketSharp } from "react-icons/io5";
import NavbarLink from "./Navbar Links/NavbarLink.jsx";
import './Navbar.css'
import { useNavigate } from "react-router-dom";

function Navbar() {

    const navigate = useNavigate();
    const handleOnLogoClick = () => navigate('/');
    const handleOnLoginButtonClick = () => navigate('/login');
    const handleOnSignupButtonClick = () => navigate('/signup');

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
                <NavbarLink icon={ <IoBedSharp /> } text="Værelser"/>
                <NavbarLink icon={ <IoBookmarksSharp /> } text="Mine Bookings"/>
                <NavbarLink icon={ <IoTicketSharp /> } text="Tickets"/>

                <button className="navbar-login-button"
                        onClick={handleOnLoginButtonClick}>Login</button>

                <button className="navbar-signup-button"
                        onClick={handleOnSignupButtonClick}>Sign up</button>
            </div>
        </div>
    )
}

export default Navbar
