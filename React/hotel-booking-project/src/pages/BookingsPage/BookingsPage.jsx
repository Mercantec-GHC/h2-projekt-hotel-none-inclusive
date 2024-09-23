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

                const { email } = JSON.parse(atob(token.split('.')[1])); // Decode JWT token to get email
                console.log(email);
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
                        <p>Booking Date: {new Date(booking.BookingDate).toLocaleDateString()}</p>
                        <p>Start Date: {new Date(booking.BookingStartDate).toLocaleDateString()}</p>
                        <p>End Date: {new Date(booking.BookingEndDate).toLocaleDateString()}</p>
                        <p>Room: {booking.RoomInfo.name}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BookingsPage;
