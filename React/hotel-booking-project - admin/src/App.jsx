import {Route, Routes, useNavigate} from "react-router-dom";
import './App.css'
import Home from "./pages/Home/Home.jsx";
import Footer from "./components/Footer/Footer.jsx";
import Navbar from "./components/Navbar/Navbar.jsx";
import LoginPage from "./pages/Login & Signup/LoginPage.jsx"
import SignupPage from "./pages/Login & Signup/SignupPage.jsx";
import Rooms from "./pages/Rooms/Rooms.jsx";
import AllRooms from "./pages/Rooms/AllRooms.jsx";
import BookingsPage  from "./pages/BookingsPage/BookingsPage.jsx";
import Contact from "./pages/Contact/Contact.jsx";
import UsersPage from "./pages/Users/UsersPage.jsx";
import {useEffect} from "react";
import {useAuth} from "./context/AuthContext.jsx";
import CreateRoomPage from "./pages/CreateRoom/CreateRoomPage.jsx";


function App() {

    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();

    useEffect(() => {
        if (!isAuthenticated) {
            navigate('/login');
        }
    }, [isAuthenticated, navigate]);

    return (
        <>
            <div className="container">
                <Navbar />

                <div className="page-container">
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/signup" element={<SignupPage />} />
                        <Route path="/rooms" element={<Rooms />} />
                        <Route path="/all-rooms" element={<AllRooms />} />
                        <Route path="/bookings" element={<BookingsPage />} />
                        <Route path="/contact" element={<Contact />} />
                        <Route path="/users" element={<UsersPage />} />
                        <Route path="/create-room" element={<CreateRoomPage />} />
                    </Routes>
                </div>

                <Footer />
            </div>
        </>
    )
}

export default App
