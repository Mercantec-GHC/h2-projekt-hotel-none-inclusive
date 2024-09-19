import './NavbarLink.css'
import { useNavigate } from "react-router-dom";

function NavbarLink({ icon, text, link }) {

    const navigate = useNavigate();
    const handleOnClick = () => navigate(link);

    return (
        <div className="navbar-nav-link-container" onClick={handleOnClick}>
            {icon}
            <p>{text}</p>
        </div>
    )
}

export default NavbarLink
