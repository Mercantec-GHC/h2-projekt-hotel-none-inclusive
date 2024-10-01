import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './BookingsPage.css';
import { MenuItem, TextField, Select} from "@mui/material";

const BookingsPage = () => {
    const [bookings, setBookings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchMonth, setSearchMonth] = useState('');

    const monthOptions = [
        { value: '1', label: 'Januar' },
        { value: '2', label: 'Februar' },
        { value: '3', label: 'Marts' },
        { value: '4', label: 'April' },
        { value: '5', label: 'Maj' },
        { value: '6', label: 'Juni' },
        { value: '7', label: 'Juli' },
        { value: '8', label: 'August' },
        { value: '9', label: 'September' },
        { value: '10', label: 'Oktober' },
        { value: '11', label: 'November' },
        { value: '12', label: 'December' },
    ];

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

    const updatePaymentStatus = async (bookingId, newStatus) => {
        try {
            await axios.put(`https://localhost:7207/api/Booking/${bookingId}/paymentStatus`, { paymentStatus: newStatus });
            setBookings(bookings.map(booking => {
                if (booking.id === bookingId) {
                    return {
                        id: booking.id,
                        bookingDate: booking.bookingDate,
                        bookingStartDate: booking.bookingStartDate,
                        bookingEndDate: booking.bookingEndDate,
                        roomInfo: booking.roomInfo,
                        userInfo: booking.userInfo,
                        paymentStatus: newStatus
                    };
                }
                return booking;
            }));
        } catch (err) {
            setError(err.message);
        }
    };

    const filteredBookings = bookings.filter(booking => {
        const matchesSearchQuery =
            (booking.userInfo && booking.userInfo.email.toLowerCase().includes(searchQuery.toLowerCase())) ||
            (booking.roomInfo && booking.roomInfo.roomType.toLowerCase().includes(searchQuery.toLowerCase()));

        const bookingMonth = (new Date(booking.bookingStartDate).getMonth() + 1).toString();
        const matchesSearchMonth = searchMonth === "" || bookingMonth === searchMonth;

        // Return true only if both conditions are satisfied
        return matchesSearchQuery && matchesSearchMonth;
    });


    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div className="bookings-page">
            <h1>Alle Bookings</h1>
            <TextField
                label="Søg med email eller værelsestype"
                variant="outlined"
                fullWidth
                margin="normal"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
            />
            {/* Month Select */}
            <Select
                label="Søg med måned"
                value={searchMonth}
                onChange={(e) => setSearchMonth(e.target.value)}
                fullWidth
                displayEmpty
                variant="outlined"
                margin="normal"
            >
                <MenuItem value="">Alle måneder</MenuItem> {/* Option for selecting all months */}
                {monthOptions.map(month => (
                    <MenuItem key={month.value} value={month.value}>
                        {month.label}
                    </MenuItem>
                ))}
            </Select>
            <ul>
                {filteredBookings.map((booking, index) => (
                    <li key={index}>
                        <p>Booking Dato: {new Date(booking.bookingDate).toLocaleDateString()}</p>
                        <p>Start Dato: {new Date(booking.bookingStartDate).toLocaleDateString()}</p>
                        <p>Slut Dato: {new Date(booking.bookingEndDate).toLocaleDateString()}</p>
                        <p>Værelses-nummer: {booking.roomInfo ? booking.roomInfo.roomNumber : 'N/A'}</p>
                        <p>Værelses Type: {booking.roomInfo ? booking.roomInfo.roomType : 'N/A'}</p>
                        <p>Email: {booking.userInfo ? `${booking.userInfo.email} ` : 'N/A'}</p>
                        <p>Total pris: {booking.totalPrice} DKK</p>
                        <p style={{color: booking.paymentStatus ? 'green' : 'red'}}>Betalings
                            status: {booking.paymentStatus ? 'Betalt' : 'Ingen betaling modtaget'}</p>
                        <input
                            type="checkbox"
                            checked={booking.paymentStatus}
                            onChange={(e) => updatePaymentStatus(booking.id, e.target.checked)}
                        />
                        <label>{booking.paymentStatus ? 'Fjern betaling' : 'Tilføj betaling'}</label>
                        <button onClick={() => handleDelete(booking.id)}>Slet Booking</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BookingsPage;
