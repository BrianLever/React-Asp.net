import React from 'react';
import { NUMBER_TO_SHOW, NEXT_TEXT, PREVIOUS_TEXT, FIRST_TEXT, LAST_TEXT, 
         List, EllipsisButtonTextStyle, PageButtonTextStyle
} from './styledComponents';
import  { usePagination } from '@material-ui/lab/Pagination';
import { Button } from '@material-ui/core';
import { styled } from '@material-ui/styles';

export const CustomEllipsisButton = styled(Button)({
    padding: '0.575em 0.85em 0.575em 0.85em',
    '&:hover': {
      backgroundColor: 'rgb(237,237,242)'
    },
});

export const CustomPageButton = styled(Button)({
    padding: '13px 8px',
    minWidth: '3.5em'
});

export interface IScreendoxPagination {
    currentPage: number;
    count: number; 
    disabled: boolean | undefined;
    defaultPage: number;
    onChange?: (event: React.ChangeEvent<unknown>, page: number) => void;
}


const ScreendoxPagination = (props: IScreendoxPagination) => {
    
    const { currentPage, count, disabled, defaultPage } = props;

    const { items } = usePagination({
        count: count,
    });

   return (
    <nav>
        <List>
            <li> <CustomEllipsisButton
                    disabled={currentPage === 1}
                    onClick={(e) => {
                        props.onChange && props.onChange(e, 1);
                    }}
                 >
                     <EllipsisButtonTextStyle selected={currentPage === 1}>{ FIRST_TEXT }</EllipsisButtonTextStyle>
                </CustomEllipsisButton>
            </li>
            {items.map(({ page, type, selected, ...item }, index) => {
                let children = null;
                console.log(page, type, selected, item)
                if (type === 'start-ellipsis' || type === 'end-ellipsis') {
                    children = '…';
                } else if (type === 'page') {
                    children = (
                        <CustomPageButton
                            style={{ backgroundColor: currentPage === page?'rgb(237,237,242)':''}}
                            disabled={currentPage === page}
                            onClick={(e) => {
                                props.onChange && props.onChange(e, page);
                            }}
                        >
                            <PageButtonTextStyle selected={currentPage === page}>{page}</PageButtonTextStyle>
                        </CustomPageButton>
                    );
                } else if(type === 'previous') {
                    const isPrevious = (currentPage - NUMBER_TO_SHOW) < 1;
                    children = (
                        <CustomEllipsisButton
                            disabled={isPrevious}
                            onClick={(e) => {
                                props.onChange && props.onChange(e, currentPage - NUMBER_TO_SHOW);
                            }}
                        >
                            <EllipsisButtonTextStyle selected={isPrevious}>{PREVIOUS_TEXT} {NUMBER_TO_SHOW}</EllipsisButtonTextStyle>
                        </CustomEllipsisButton>
                    );
                } else if(type === 'next') {
                    const isNext = (currentPage + NUMBER_TO_SHOW) > count;
                    children = (
                        <CustomEllipsisButton disabled={isNext}
                            onClick={(e) => {
                                props.onChange && props.onChange(e, currentPage + NUMBER_TO_SHOW);
                            }}
                        >
                            <EllipsisButtonTextStyle selected={isNext}>{NEXT_TEXT} {NUMBER_TO_SHOW}</EllipsisButtonTextStyle>
                        </CustomEllipsisButton>
                    );
                }
                
                return <li key={index}>{children}</li>;
            })}
            <li> <CustomEllipsisButton 
                    disabled={currentPage === count}
                    onClick={(e) => {
                        props.onChange && props.onChange(e, count);
                    }}
                >
                    <EllipsisButtonTextStyle selected={currentPage === count}>{ LAST_TEXT }</EllipsisButtonTextStyle> 
                </CustomEllipsisButton>
            </li>
        </List>
    </nav>    
   )
}

export default ScreendoxPagination;