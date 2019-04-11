// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import * as React from 'react'

import './ActionButton.scss'

export class ActionButton extends React.PureComponent<{
    iconName: string
    title: string
    onClick?: () => void
}> {
    render() {
        return (
            <button
                className="ActionButton Small"
                title={this.props.title}
                onClick={this.props.onClick}>
                <svg className={`ActionButton-${this.props.iconName}`} viewBox="0 0 16 16">
                    {this.renderIcon()}
                </svg>
            </button>
        )
    }

    private renderIcon() {
        switch (this.props.iconName) {
            case "CodeCell-Running":
                return (
                    <g>
                        <g stroke="none" className="CodeCell-Running-Group">
                            <path className="CodeCell-Running-Ring1" d="M8,0 C3.581722,0 0,3.581722 0,8 C0,12.418278 3.581722,16 8,16 C12.418278,16 16,12.418278 16,8 L15,8 C15,11.8659932 11.8659932,15 8,15 C4.13400675,15 1,11.8659932 1,8 C1,4.13400675 4.13400675,1 8,1 L8,0 Z"/>
                            <path className="CodeCell-Running-Ring2" d="M2,8 C2,4.6862915 4.6862915,2 8,2 C11.3137085,2 14,4.6862915 14,8 C14,11.3137085 11.3137085,14 8,14 L8,13 C10.7614237,13 13,10.7614237 13,8 C13,5.23857625 10.7614237,3 8,3 C5.23857625,3 3,5.23857625 3,8 L2,8 Z"/>
                            <path className="CodeCell-Running-Ring3" d="M8,4 C5.790861,4 4,5.790861 4,8 C4,10.209139 5.790861,12 8,12 C10.209139,12 12,10.209139 12,8 L11,8 C11,9.65685425 9.65685425,11 8,11 C6.34314575,11 5,9.65685425 5,8 C5,6.34314575 6.34314575,5 8,5 L8,4 Z"/>
                        </g>
                        <g stroke="none" className="CodeCell-Running-Cancel-Group">
                            <path d="M8,6.58578644 L10.1245748,4.46121168 C10.5133017,4.07248475 11.142287,4.07121914 11.5355339,4.46446609 C11.9260582,4.85499039 11.9204373,5.49377626 11.5387883,5.87542525 L9.41421356,8 L11.5387883,10.1245748 C11.9275152,10.5133017 11.9287809,11.142287 11.5355339,11.5355339 C11.1450096,11.9260582 10.5062237,11.9204373 10.1245748,11.5387883 L8,9.41421356 L5.87542525,11.5387883 C5.48669832,11.9275152 4.85771305,11.9287809 4.46446609,11.5355339 C4.0739418,11.1450096 4.0795627,10.5062237 4.46121168,10.1245748 L6.58578644,8 L4.46121168,5.87542525 C4.07248475,5.48669832 4.07121914,4.85771305 4.46446609,4.46446609 C4.85499039,4.0739418 5.49377626,4.0795627 5.87542525,4.46121168 L8,6.58578644 Z M8,16 C3.581722,16 0,12.418278 0,8 C0,3.581722 3.581722,0 8,0 C12.418278,0 16,3.581722 16,8 C16,12.418278 12.418278,16 8,16 Z M8,15 C11.8659932,15 15,11.8659932 15,8 C15,4.13400675 11.8659932,1 8,1 C4.13400675,1 1,4.13400675 1,8 C1,11.8659932 4.13400675,15 8,15 Z"></path>
                        </g>
                    </g>
                )
            case "CodeCell-Run":
                return (
                    <g stroke="none">
                        <path d="M8,16 C3.581722,16 0,12.418278 0,8 C0,3.581722 3.581722,0 8,0 C12.418278,0 16,3.581722 16,8 C16,12.418278 12.418278,16 8,16 Z M8,15 C11.8659932,15 15,11.8659932 15,8 C15,4.13400675 11.8659932,1 8,1 C4.13400675,1 1,4.13400675 1,8 C1,11.8659932 4.13400675,15 8,15 Z M11,8 L6,12 L6,4 L11,8 Z"/>
                    </g>
                )
        }

        return false;
    }
}