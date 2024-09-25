import { useState } from 'react';

const Contact = () => {
    // Set the default values
    const [emailToId] = useState('noreply@leeloo.dk');
    const [emailToName] = useState('Support');
    const [emailSubject, setEmailSubject] = useState('');
    const [emailBody, setEmailBody] = useState('');
    const [response, setResponse] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const emailData = {
            emailToId,
            emailToName,
            emailSubject,
            emailBody
        };

        try {
            const res = await fetch('https://localhost:7207/Mail', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain'
                },
                body: JSON.stringify(emailData)
            });

            if (res.ok) {
                // const result = await res.json();
                setResponse('Email sent successfully!');
                // Clear the subject and body fields after sending
                setEmailSubject('');
                setEmailBody('');
            } else {
                setResponse('Failed to send email.');
            }
        } catch (error) {
            console.error('Error:', error);
            setResponse('An error occurred while sending the email.');
        }
    };

    return (
        <div className="backgroundLogin">
            <div className="login-container">
                <div>
                    <form onSubmit={handleSubmit}>
                        {/* Removed Email To (ID) and Email To (Name) fields */}
                        <div>
                            <label>Subject: </label>
                            <input
                                type="text"
                                value={emailSubject}
                                onChange={(e) => setEmailSubject(e.target.value)}
                                required
                            />
                        </div>
                        <div>
                            <label>Your Message: </label>
                            <textarea
                                value={emailBody}
                                onChange={(e) => setEmailBody(e.target.value)}
                                required
                            />
                        </div>
                        <button type="submit">Send Email</button>
                    </form>
                    {response && <p>{response}</p>}
                </div>
            </div>
        </div>
    );
};

export default Contact;
