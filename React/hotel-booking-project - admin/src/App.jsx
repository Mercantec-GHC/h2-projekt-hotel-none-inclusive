import { Route, Routes } from "react-router-dom";
import './App.css'
import Home from "./pages/Home/Home.jsx";
import Footer from "./components/Footer/Footer.jsx";
import Navbar from "./components/Navbar/Navbar.jsx";
import LoginPage from "./pages/Login & Signup/LoginPage.jsx"
import SignupPage from "./pages/Login & Signup/SignupPage.jsx";
import Rooms from "./pages/Rooms/Rooms.jsx";
import BookingsPage  from "./pages/BookingsPage/BookingsPage.jsx";
import Contact from "./pages/Contact/Contact.jsx";


function App() {

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
                        <Route path="/bookings" element={<BookingsPage />} />
                        <Route path="/contact" element={<Contact />} />
                    </Routes>
                </div>

                <Footer />
            </div>
        </>
    )
}

export default App
