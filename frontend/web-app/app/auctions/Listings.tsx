'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { Auction } from '@/types';
import { getData } from '../actions/auctionActions';

export default function Listings() {
    const [auctions, setAuctions] = useState<Auction[]>([]);
    const [pageCount, setPageCount] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);

    useEffect(() => {
        getData(pageNumber).then((data) => {
            setAuctions(data.result);
            setPageCount(data.pageCount);
        });
    }, [pageNumber]);

    if(auctions.length === 0) {
        return <h3>Loading...</h3>
    }

    return (
        <>
            <div className='grid grid-cols-4 gap-6'>
                {auctions && auctions.map((auction) => (
                    <AuctionCard auction={auction}  key={auction.id} />
                ))}
            </div>
            <div className='flex justify-center mt-4'>
                <AppPagination 
                    currentPage={pageNumber} 
                    pageCount={pageCount}
                    pageChanged={setPageNumber} />
            </div>
        </>
    )
}
