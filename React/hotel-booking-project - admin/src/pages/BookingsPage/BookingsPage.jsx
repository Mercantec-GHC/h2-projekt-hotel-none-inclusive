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
                const response = await axios.get('https://localhost:7207/api/Booking');
                setBookings(response.data);
                setLoading(false);
            } catch (err) {
                setError(err.message);
                setLoading(false);
            }
        };
        fetchBookings();
    }, []);

    const handleDelete = async (bookingId) => {
        if (!bookingId) {
            setError('Invalid booking ID');
            return;
        }

        try {
            await axios.delete(`https://localhost:7207/api/Booking/${bookingId}`);
            setBookings(bookings.filter(booking => booking.id !== bookingId));
        } catch (err) {
            setError(err.message);
        }
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div className="bookings-page">
            <h1>Alle Bookings</h1>
            <ul>
                {bookings.map((booking, index) => (
                    <li key={index}>
                        <p>Booking Dato: {new Date(booking.bookingDate).toLocaleDateString()}</p>
                        <p>Start Dato: {new Date(booking.bookingStartDate).toLocaleDateString()}</p>
                        <p>Slut Dato: {new Date(booking.bookingEndDate).toLocaleDateString()}</p>
                        <p>Værelses-nummer: {booking.roomInfo ? booking.roomInfo.roomNumber : 'N/A'}</p>
                        <p>Værelses Type: {booking.roomInfo ? booking.roomInfo.roomType : 'N/A'}</p>
                        <p>Email: {booking.userInfo ? `${booking.userInfo.email} ` : 'N/A'}</p>
                        <button onClick={() => handleDelete(booking.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BookingsPage;
