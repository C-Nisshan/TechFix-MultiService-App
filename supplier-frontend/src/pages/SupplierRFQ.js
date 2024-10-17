import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/SupplierRFQ.css';

const SupplierRFQ = () => {
  const [rfqs, setRFQs] = useState([]);
  const [responseDetails, setResponseDetails] = useState({ price: '', deliveryTime: '', quoteItems: [] });
  const [showModal, setShowModal] = useState(false);
  const [selectedRFQ, setSelectedRFQ] = useState(null);

  useEffect(() => {
    fetchRFQs();
  }, []);

  const fetchRFQs = async () => {
    try {
      const res = await axios.get('http://localhost:5001/api/rfq');
      setRFQs(res.data);
    } catch (error) {
      console.error('Error fetching RFQs:', error);
      toast.error('Error fetching RFQs');
    }
  };

  
  const handleAcceptReject = async (rfq, status) => {
    console.log('RFQ : ', rfq);
    console.log('Status : ', status);

    if (!status) {
        toast.error('Status is required');
        return;
    }

    try {
        const payload = { Status: status === 'ACCEPTED' ? 'APPROVED' : status }; // Map to APPROVED if ACCEPTED
        console.log('Payload:', payload);

        await axios.put(`http://localhost:5001/api/rfq/${rfq.rfqId}/respond`, payload);
        toast.success(`RFQ ${status.toLowerCase()} successfully!`);
        fetchRFQs(); // Refresh the RFQ list after status update
    } catch (error) {
        console.error(`Error ${status.toLowerCase()}ing RFQ:`, error.response?.data || error.message);
        toast.error(`Error ${status.toLowerCase()}ing RFQ`);
    }
};


  const handleQuoteSubmit = async (e) => {
    e.preventDefault();
    if (!selectedRFQ) return;

    const quote = {
      rfqId: selectedRFQ.rfqId,
      totalPrice: responseDetails.price,
      deliveryTime: responseDetails.deliveryTime,
      quoteItems: responseDetails.quoteItems
    };

    try {
      await axios.post(`http://localhost:5001/api/quote/${selectedRFQ.rfqId}/createQuote`, quote);
      toast.success('Quote submitted successfully!');
      fetchRFQs();
      handleModalClose();
    } catch (error) {
      console.error('Error submitting quote:', error);
      toast.error('Error submitting quote');
    }
  };

  const handleQuoteItemChange = (index, field, value) => {
    const updatedQuoteItems = [...responseDetails.quoteItems];
    updatedQuoteItems[index] = {
      ...updatedQuoteItems[index],
      [field]: value
    };
    setResponseDetails({ ...responseDetails, quoteItems: updatedQuoteItems });
  };

  const handleModalClose = () => setShowModal(false);

  const handleRespond = (rfq) => {
    setSelectedRFQ(rfq);
    const quoteItems = rfq.rfqItems.map(item => ({
      productId: item.productId,
      availableQuantity: '',
      quotedPrice: '',
      discount: ''
    }));
    setResponseDetails({ price: '', deliveryTime: '', quoteItems });
    setShowModal(true);
  };

  return (
    <Container className="supplier-rfq-container mt-5">
      <ToastContainer autoClose={5000} />
      <h1 className="text-center mb-4">Request For Quote</h1>

      <Row className="rfq-list">
        {rfqs.map((rfq) => (
          <Col md={4} key={rfq.rfqId} className="mb-4">
            <Card className="supplier-rfq-card shadow-sm">
              <Card.Body>
                <Card.Title>RFQ ID: {rfq.rfqId}</Card.Title>
                <Card.Text><strong>Created Date:</strong> {new Date(rfq.creationDate).toLocaleDateString()}</Card.Text>
                <Card.Text><strong>Status:</strong> 
                  <span className={`status-badge ${rfq.status.toLowerCase()}`}>{rfq.status}</span>
                  {console.log(rfq.status)};
                </Card.Text>

                <Card.Text><strong>Items:</strong></Card.Text>
                <ul className="rfq-items-list">
                  {rfq.rfqItems.map((item) => (
                    <li key={item.rfqItemId}>
                      <strong>Product:</strong> {item.productName} <br />
                      <strong>Quantity:</strong> {item.requestedQuantity}
                    </li>
                  ))}
                </ul>

                {/* Accept/Reject buttons */}
                {rfq.status === 'PENDING' && (
                  <div className="action-buttons">
                    <Button variant="success" className='mb-2' onClick={() => handleAcceptReject(rfq, 'ACCEPTED')}>
                      Accept
                    </Button>
                    <Button variant="danger" onClick={() => handleAcceptReject(rfq, 'REJECTED')}>
                      Reject
                    </Button>
                  </div>
                )}

                {['ACCEPTED', 'APPROVED'].includes(rfq.status) && (
                  <Button variant="primary" onClick={() => handleRespond(rfq)} className="respond-btn">
                    Submit Quote
                  </Button>
                )}
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Modal for quote form */}
      <Modal show={showModal} onHide={handleModalClose}>
        <Modal.Header closeButton>
          <Modal.Title>Submit Quote for RFQ</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleQuoteSubmit}>
            <Form.Group controlId="quotePrice" className="mb-3">
              <Form.Label>Price</Form.Label>
              <Form.Control
                type="number"
                placeholder="Enter total price"
                value={responseDetails.price}
                onChange={(e) => setResponseDetails({ ...responseDetails, price: e.target.value })}
                required
              />
            </Form.Group>

            <Form.Group controlId="quoteDeliveryTime" className="mb-3">
              <Form.Label>Delivery Time (days)</Form.Label>
              <Form.Control
                type="number"
                placeholder="Enter delivery time"
                value={responseDetails.deliveryTime}
                onChange={(e) => setResponseDetails({ ...responseDetails, deliveryTime: e.target.value })}
                required
              />
            </Form.Group>

            {/* Loop through RFQ Items for quoting */}
            {selectedRFQ && selectedRFQ.rfqItems.map((item, index) => (
              <div key={item.rfqItemId}>
                <h5>Product: {item.productName}</h5>
                <Form.Group controlId={`quoteItem-${index}`} className="mb-3">
                  <Form.Label>Available Quantity</Form.Label>
                  <Form.Control
                    type="number"
                    placeholder="Enter available quantity"
                    value={responseDetails.quoteItems[index]?.availableQuantity || ''}
                    onChange={(e) => handleQuoteItemChange(index, 'availableQuantity', e.target.value)}
                  />
                  <Form.Label>Quoted Price</Form.Label>
                  <Form.Control
                    type="number"
                    placeholder="Enter quoted price"
                    value={responseDetails.quoteItems[index]?.quotedPrice || ''}
                    onChange={(e) => handleQuoteItemChange(index, 'quotedPrice', e.target.value)}
                  />
                  <Form.Label>Discount (%)</Form.Label>
                  <Form.Control
                    type="number"
                    placeholder="Enter discount"
                    value={responseDetails.quoteItems[index]?.discount || ''}
                    onChange={(e) => handleQuoteItemChange(index, 'discount', e.target.value)}
                  />
                </Form.Group>
              </div>
            ))}

            <Button variant="primary" type="submit">
              Submit Quote
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default SupplierRFQ;
