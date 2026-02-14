import {
  MediaPlayer,
  MediaPlayerVideo,
  MediaPlayerLoading,
  MediaPlayerError,
  MediaPlayerVolumeIndicator,
  MediaPlayerControls,
  MediaPlayerControlsOverlay,
  MediaPlayerSeek,
  MediaPlayerPlay,
  MediaPlayerSeekBackward,
  MediaPlayerSeekForward,
  MediaPlayerVolume,
  MediaPlayerTime,
  MediaPlayerPlaybackSpeed,
  MediaPlayerPiP,
  MediaPlayerFullscreen,
  MediaPlayerProps,
} from "@/shared/components/ui/media-player";

export const VideoPlayer = ({
  url,
  ...props
}: MediaPlayerProps & { url: string }) => {
  return (
    <MediaPlayer className="w-full h-full" {...props}>
      <MediaPlayerVideo
        src={url}
        className="w-full h-full object-contain bg-black"
      />
      <MediaPlayerLoading />
      <MediaPlayerError />
      <MediaPlayerVolumeIndicator />
      <MediaPlayerControls className="flex-col items-start gap-2.5">
        <MediaPlayerControlsOverlay />
        <MediaPlayerSeek />
        <div className="flex w-full items-center gap-2">
          <div className="flex flex-1 items-center gap-2">
            <MediaPlayerPlay />
            <MediaPlayerSeekBackward />
            <MediaPlayerSeekForward />
            <MediaPlayerVolume expandable />
            <MediaPlayerTime />
          </div>
          <div className="flex items-center gap-2">
            <MediaPlayerPlaybackSpeed />
            <MediaPlayerPiP />
            <MediaPlayerFullscreen />
          </div>
        </div>
      </MediaPlayerControls>
    </MediaPlayer>
  );
};
