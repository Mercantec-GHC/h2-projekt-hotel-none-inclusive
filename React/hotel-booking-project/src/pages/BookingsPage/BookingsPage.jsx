import { useEffect, useState } from 'react';
import axios from 'axios';
import './BookingsPage.css';

const BookingsPage = () => {
    const [bookings, setBookings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchBookings = async () => {
            try {
                const token = localStorage.getItem('token');
                if (!token) {
                    throw new Error('No token found');
                }

                const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT token to get payload
                const email = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
                if (!email) {
                    throw new Error('Email not found in token');
                }

                const response = await axios.get(`https://localhost:7207/api/Booking/user/${email}`);
                setBookings(response.data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchBookings();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div className="bookings-page">
            <h1>Mine Bookings</h1>
            <ul>
                {bookings.map((booking, index) => (
                    <li key={index}>
                        <p>Booking Date: {new Date(booking.bookingDate).toLocaleDateString()}</p>
                        <p>Start Date: {new Date(booking.bookingStartDate).toLocaleDateString()}</p>
                        <p>End Date: {new Date(booking.bookingEndDate).toLocaleDateString()}</p>
                        <p>Room: {booking.roomInfo ? booking.roomInfo.roomNumber : 'N/A'}</p>
                        <p>Room Type: {booking.roomInfo ? booking.roomInfo.roomType : 'N/A'}</p>
                        <p>Description: {booking.roomInfo ? booking.roomInfo.description : 'N/A'}</p>
                        <img src={booking.roomInfo ? booking.roomInfo.imageURL : ''} alt="Room" />
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BookingsPage;
